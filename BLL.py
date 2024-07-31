# import pandas as pd
# import numpy as np
# # import pymongo as pm
# from flask import Flask, jsonify, request, flash, send_file, Response
# from flask_cors import CORS, cross_origin
# import start
# app = Flask(__name__)
# CORS(app, supports_credentials=True)
# # client = pm.MongoClient("mongodb://localhost:27017/")
#
# @app.route('/img', methods=['POST'])
# @cross_origin(supports_credentials=True)
# # def get_maze():
# #     # if 'file' not in request.files:
# #     #     flash('No file part')
# #     #     return ""
# #     file = request.files['file']
# #     print(file)
# #     if file.filename == "":
# #         print("dfghj")
# #         return ""
# #     if file:
# #         print("Hello")
# #         filename = secure_filename(file.filename)
# #         print(type(file))
# #         main(file)
# #         # file.save(os.path.join(app.config['UPLOAD_FILES'], f'img_123.jpg'))
# #         return jsonify("good")
#
# def upload_file():
#     if 'file' not in request.files:
#         return jsonify({"error": "No file part"}), 400
#     file = request.files['file']
#     if file.filename == '':
#         return jsonify({"error": "No selected file"}), 400
#     if file:
#         filename = secure_filename(file.filename)
#         file.save(os.path.join(app.config['UPLOAD_FOLDER'], filename))
#         return jsonify({"message": "File uploaded successfully"}), 200
#
#
# if __name__ == '__main__':
#     app.secret_key = 'super secret key'
#     app.config['SESSION_TYPE'] = 'filesystem'
#     app.run(debug=True)


import os
import random

from flask import Flask, jsonify, request
from flask_cors import CORS, cross_origin
from werkzeug.utils import secure_filename

from start import main

app = Flask(__name__)
CORS(app, supports_credentials=True)

# נתיב לשמירת הקבצים שהועלו
app.config['UPLOAD_FOLDER'] = 'uploads'  # החלף את זה עם הנתיב שבו תרצה לשמור את הקבצים


@app.route('/img', methods=['POST'])
@cross_origin(supports_credentials=True)
def upload_file():
    if 'file' not in request.files:
        return jsonify({"error": "No file part"}), 400
    file = request.files['file']
    if file.filename == '':
        return jsonify({"error": "No selected file"}), 400
    if file:
        directory = r"C:\Users\User\Desktop\angular\src\assets\pic\img_solve"
        random_number = random.randint(0, 999999)
        filename = str(random_number)+"_"+secure_filename(file.filename)
        file.save(os.path.join(directory, filename))
        print(os.path.join(directory, filename))
        mazeS = main(os.path.join(directory, filename))
        try:
            filepath = os.path.join(directory, filename)
            mazeS.save(filepath)
        except FileNotFoundError:
            print("יש בעיה!!!")
        except PermissionError:
            print("בעיה חמורה!!")
        except Exception as e:
            print("משהו אחרררררררררררר")
        print(file)
        print(filename)
        print(type(filename))
        m = os.path.join(directory, filename)
        print(m)
        # return send_file(m, mimetype='image/png'), 200
        return jsonify({'image_path': filename})
        # return jsonify({'image_path': fr"C:\Users\User\Desktop\angular\src\assets\pic\img_solve\{m}"})


if __name__ == '__main__':
    # ודא שהתיקייה לשמירת הקבצים קיימת
    if not os.path.exists(app.config['UPLOAD_FOLDER']):
        os.makedirs(app.config['UPLOAD_FOLDER'])

    app.secret_key = 'super secret key'
    app.config['SESSION_TYPE'] = 'filesystem'
    app.run(debug=True)
