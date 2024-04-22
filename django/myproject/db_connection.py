import pymongo

url = 'mongodb+srv://admin:xN3zlJYmEYZZhN64@cluster0.zswh0fl.mongodb.net/'
client = pymongo.MongoClient(url)

db = client['test']