import json

import numpy as np
import requests
from PIL import Image


def find_point_start(pixel_matrix, width, height):
    start = (-1, -1)
    for i in range(width):
        for j in range(height):
            if pixel_matrix[i, j] == 0:
                k = j
                while (k < height and pixel_matrix[i, k] == 0):
                    k += 1
                l = i
                while (l < height and pixel_matrix[l, j] == 0):
                    l += 1
                if k - j > height * 0.5 or l - i > width * 0.5:
                    start = (i, j)
                    break
        if start != (-1, -1):
            break
    return start


def find_point_end(pixel_matrix, width, height):
    finish = (-1, -1)
    for j in range(height - 1, -1, -1):  # גובה
        for i in range(width - 1, -1, -1):  # רוחב
            if pixel_matrix[i, j] == 0:  # הצירים כאן נכונים
                k = j
                while k >= 0 and pixel_matrix[i, k] == 0:
                    k -= 1
                l = i
                while l >= 0 and pixel_matrix[l, j] == 0:
                    l -= 1
                if (j - k) > height * 0.5 or (i - l) > width * 0.5:
                    finish = (i, j)
                    return finish
    return finish


def GetMaze(pixel_matrix, binary_image):
    # הסרה של השולים
    width, height = binary_image.size
    flag = False
    # צפון
    indexNorth = None
    # דרום
    indexSout = None
    # מערב
    indexWest = None
    # מזרח
    indexEast = None

    for x in range(width):
        for y in range(height):
            if pixel_matrix[x, y] == 0:
                indexNorth = x
                indexWest = y
                flag = True
                break
        if flag is True:
            break

    for i in range(width - 1, -1, -1):
        for j in range(height - 1, -1, -1):
            if pixel_matrix[i, j] == 0:
                indexSout = i
                indexEast = j
                break
        if indexEast is not None:
            break

    w, h = indexSout - indexNorth + 1, indexEast - indexWest + 1
    maze = np.empty((w, h), dtype=int)
    i, j = 0, 0
    for i in range(indexNorth, indexSout + 1, 1):
        for j in range(indexWest, indexEast + 1, 1):
            # print(pixel_matrix[i, j], end=" ")
            maze[i - indexNorth, j - indexWest] = pixel_matrix[i, j]
            # print(maze[i - indexNorth, j - indexWest], end=" ")
        # print()

    return maze


def get_matrix_maze(image_path):
    # קריאת תמונה
    image = Image.open(image_path).convert("L")
    binary_image = image.point(lambda pixel: 0 if pixel < 128 else 1, mode="1")
    w, h = binary_image.size
    pixel_matrix = binary_image.load()
    start = find_point_start(pixel_matrix, w, h)
    end = find_point_end(pixel_matrix, w, h)
    # מוודא שהנקודות נמצאו
    if start == (-1, -1) or end == (-1, -1):
        print("Couldn't find start or end point.")
        return
    # בניית תמונה מחודשת
    reconstructed_image = Image.new("1", binary_image.size)
    reconstructed_image = reconstructed_image.point(lambda pixel: 1 if pixel < 128 else 1, mode="1")
    reconstructed_pixels = reconstructed_image.load()
    for i in range(start[0], end[0] + 1):
        for j in range(start[1], end[1] + 1):
            reconstructed_pixels[i, j] = pixel_matrix[i, j]
    # reconstructed_image.show()
    p = GetMaze(reconstructed_pixels, binary_image)
    # המרת התמונה לקטנה
    # reconstructed_image = Image.new("1", p.shape)
    # reconstructed_image = reconstructed_image.point(lambda pixel: 1 if pixel < 128 else 1, mode="1")
    # reconstructed_pixels = reconstructed_image.load()
    # w2, h2 = p.shape
    # for i in range(w2):
    #     for j in range(h2):
    #         reconstructed_pixels[i, j] = int(p[i, j])
    # reconstructed_image.show()
    reconstructed_image = Image.new("RGB", p.shape)
    # reconstructed_image = reconstructed_image.point(lambda pixel: 1 if pixel < 128 else 1, mode="1")
    reconstructed_pixels = reconstructed_image.load()
    # p[4, 4] = 3
    # w2, h2 = p.shape
    # for i in range(w2):
    #     for j in range(h2):
    #         if p[i, j] == 0:
    #             reconstructed_image.putpixel((i, j), (0, 0, 0))
    #         if p[i, j] == 1:
    #             reconstructed_image.putpixel((i, j), (255, 255, 255))
    #         if p[i, j] == 3:
    #             reconstructed_image.putpixel((i, j), (255, 0, 0))
    # reconstructed_image.show()
    return p


def get_maze_solve(matrix_solve):
    reconstructed_image = Image.new("RGB", matrix_solve.shape)
    # reconstructed_image = reconstructed_image.point(lambda pixel: 1 if pixel < 128 else 1, mode="1")
    w2, h2 = matrix_solve.shape
    for i in range(w2):
        for j in range(h2):
            if matrix_solve[i, j] == 0:
                reconstructed_image.putpixel((j, i), (0, 0, 0))
            if matrix_solve[i, j] == 1:
                reconstructed_image.putpixel((j, i), (255, 255, 255))
            if matrix_solve[i, j] == 3:
                reconstructed_image.putpixel((j, i), (255, 0, 0))

    reconstructed_image.show()
    return reconstructed_image


# פונקציה המחזירה את גודל תא אחד במבוך
def GetSub(maze):
    # if maze[1, 0] == 0 and maze[1, 1] == 1:
    #     return 1, 1
    width, height = maze.shape
    wall = 0
    common_wall = [0] * height
    for x in range(width - 1):
        for y in range(height - 1):
            if maze[x, y] == 0:
                i = x
                j = y
                wall_i = 0
                wall_j = 0
                while maze[i, y] == 0:
                    if i > width - 2:
                        break
                    wall_i += 1
                    i += 1
                while maze[x, j] == 0:
                    if j == height - 2:
                        break
                    wall_j += 1
                    j += 1
                if wall_i > wall_j:
                    wall = wall_j
                else:
                    wall = wall_i
                break
            # מערך מונים- מונה את מספר הפעמים של עובי הקיר
            if wall != 0:
                common_wall[wall] += 1

    # עברתי על המערך מונים
    # מציאת הקיר
    # התעלמות מערכים של פיקסל אחד
    max = 0
    i_max = 0
    # i-אינדקס, h- הערך באינדקס
    for i, h in enumerate(common_wall):
        if i > 0:
            if h > max:
                max = h
                i_max = i

    # הקיר של המבוך
    wall = i_max
    # transition-מעבר
    transition = 0
    common_t = [0] * height
    for x in range(width - 1):
        for y in range(height - 1):
            if maze[x, y] == 1:
                i = x
                j = y
                transition_i = 0
                transition_j = 0
                while i < width - 1 and maze[i, y] == 1:
                    if i > width - 1:
                        break
                    transition_i += 1
                    i += 1
                while maze[x, j] == 1:
                    if j == height - 1:
                        break
                    transition_j += 1
                    j += 1
                if transition_i > transition_j:
                    transition = transition_j
                else:
                    transition = transition_i
                break
            # מערך מונים- מונה את מספר הפעמים של עובי הקיר
            if transition != 0:
                common_t[transition] += 1

    # עברתי על המערך מונים
    # מציאת הקיר
    # התעלמות מערכים של פיקסל אחד
    max = 0
    i_max = 0
    c = 1
    if wall == 1:
        c = 0
    # i-אינדקס, h- הערך באינדקס
    for i, h in enumerate(common_t):
        if i > c:
            if h > max:
                max = h
                i_max = i
    # הקיר של המבוך
    transition = i_max
    return transition, wall


def main(image_path):
    # image_path = "./images_mazes/5.png"
    maze = get_matrix_maze(image_path)
    for i in range(maze.shape[0]):
        for j in range(maze.shape[1]):
            print(maze[i, j], end=" ")
        print()
    transition, wall = GetSub(maze)
    maze = maze.tolist()
    print("t", transition, "w", wall)
    maze_request = {
        "matrix_maze": maze,
        "transition": transition,
        "wall": wall
    }
    with open('maze_request.json', 'w') as json_file:
        json.dump(maze_request, json_file, indent=4)
    with open('maze_request.json', 'r') as json_file:
        maze_request = json.load(json_file)
    print(maze_request)
    url = "https://localhost:7155/s"
    headers = {'Content-Type': 'application/json'}
    response = requests.post(url, headers=headers, json=maze, verify=False)
    if response.status_code == 200:
        print("Request was successful.")
        print("Response JSON:", response.content)
    else:
        print(f"Request failed with status code {response.status_code}.")
        print("Response content:", response.content)

    url = "https://localhost:7155/solveMatrix"
    headers2 = {'Content-Type': 'application/json'}
    response2 = requests.post(url, headers=headers2, json=maze_request, verify=False)
    if response2.status_code == 200:
        print("Request was successful.")
        print("Response JSON:", response2.content)
        list_of_lists = json.loads(response2.content)

        array_2d = np.array(list_of_lists, dtype=int)

        return get_maze_solve(array_2d)
    else:
        print(f"Request failed with status code {response2.status_code}.")
        # print("Response content:", response2.content)

# if __name__ == "__main__":
#     main("././m")
