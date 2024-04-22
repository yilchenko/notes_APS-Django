
from .models import users_collection
from django.http import HttpResponse

import json
from django.http import JsonResponse
from django.contrib.auth.hashers import make_password, check_password
import jwt
from django.views.decorators.csrf import csrf_exempt
# Secret key for JWT token encoding and decoding
SECRET_KEY = 'your_secret_key'

@csrf_exempt
def register(request):
    if request.method == 'POST':
        data = json.loads(request.body)
        username = data.get('username')
        email = data.get('email')
        password = data.get('password')
        role = data.get('role')

        # Check if the username or email already exists
        if users_collection.find_one({'$or': [{'username': username}, {'email': email}]}):
            return JsonResponse({'message': 'User already exists'}, status=400)

        # Hash the password before saving it
        hashed_password = make_password(password)

        # Save user data to the database
        user_data = {
            'username': username,
            'email': email,
            'password': hashed_password,
            'role': role
        }
        users_collection.insert_one(user_data)

        return JsonResponse({'message': 'User registered successfully'}, status=201)

@csrf_exempt
def authenticate(request):
    if request.method == 'POST':
        data = json.loads(request.body)
        username = data.get('username')
        password = data.get('password')

        # Check if the user exists in the database
        user = users_collection.find_one({'username': username})
        if user and check_password(password, user['password']):
            # Generate JWT token
            token = jwt.encode({'username': username}, SECRET_KEY, algorithm='HS256')
            return JsonResponse({'token': token}, status=200)
        else:
            return JsonResponse({'message': 'Invalid credentials'}, status=401)