import csv
import matplotlib.pyplot as plt

with open('gamedata.csv', "r") as csvfile:
    reader = csv.reader(csvfile)
    next(reader)  # skip the table header
    result = dict()
    # stored data format:{'level1':{'patrolEnemy':2, 'normalEnemy':2}}
    count = 0
    for row in reader:
        if row[0] != "":  # skip blank lines
            count += 1
            patrolEnemy = int(row[2])
            normalEnemy = int(row[3])
            sceneName = row[4]
            # print(patrolEnemy, normalEnemy, sceneName)
            if sceneName in result.keys():
                result[sceneName]["patrolEnemy"] += patrolEnemy
                result[sceneName]["normalEnemy"] += normalEnemy
            else:
                result[sceneName] = dict()
                result[sceneName]["patrolEnemy"] = patrolEnemy
                result[sceneName]["normalEnemy"] = normalEnemy
    sorted_result = dict(sorted(result.items()))
    patrolEnemyArr = []
    normalEnemyArr = []
    levelArr = []
    for key in sorted_result.keys():
        levelArr.append(key)
        patrolEnemyArr.append(result[key]["patrolEnemy"]/count)
        normalEnemyArr.append(result[key]["normalEnemy"]/count)

    # print(count)
    # print(levelArr)
    # print(patrolEnemyArr)
    # print(normalEnemyArr)
    width = 0.4
    plt.bar([i for i in range(len(patrolEnemyArr))], patrolEnemyArr, width=width, label="patrolEnemyArr")
    plt.bar([i + width for i in range(len(patrolEnemyArr))], normalEnemyArr, width=width, label="normalEnemyArr")
    plt.xticks([x + width/2 for x in range(2)], levelArr)
    # 添加标题和标签
    plt.title('Average Kills in Each Level')
    plt.xlabel('Level')
    plt.ylabel('Average kills')

    plt.legend()
    plt.show()

