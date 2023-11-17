import pandas as pd
import matplotlib.pyplot as plt
import numpy as np

data = pd.read_csv('gamedata.csv')
level_dict = {}
for i in range(len(data['Scene Name'])):
    # print(data['Scene Name'][i])
    if data['Scene Name'][i] in level_dict.keys():
        level_dict[data['Scene Name'][i]]['Start'] += data['Time (Start to Transform)/seconds'][i]
        level_dict[data['Scene Name'][i]]['End'] += data['Time (Tranform to End)/seconds'][i]
        level_dict[data['Scene Name'][i]]['Possession'] += data['# of Total Possession Times'][i]
        level_dict[data['Scene Name'][i]]['Kill'] += data['# of Total Killed Enemy'][i]
        level_dict[data['Scene Name'][i]]['PossessionBullets'] += data['# of Shooting Possesion Bullet'][i]
        level_dict[data['Scene Name'][i]]['DamageBullets'] += data['# of Shooting Damage Bullet'][i]
        level_dict[data['Scene Name'][i]]['PuddleAttack'] += data['# of Becoming Puddle'][i]
        level_dict[data['Scene Name'][i]]['Win'] += data['If Wins'][i]
        level_dict[data['Scene Name'][i]]['Lose'] += data['If Loses'][i]
    else:
        level_dict[data['Scene Name'][i]] = {'Start': data['Time (Start to Transform)/seconds'][i],
                                             'End': data['Time (Tranform to End)/seconds'][i],
                                             'Possession': data['# of Total Possession Times'][i],
                                             'Kill': data['# of Total Killed Enemy'][i],
                                             'PossessionBullets': data['# of Shooting Possesion Bullet'][i],
                                             'DamageBullets': data['# of Shooting Damage Bullet'][i],
                                             'PuddleAttack': data['# of Becoming Puddle'][i],
                                             'Win': data['If Wins'][i],
                                             'Lose': data['If Loses'][i]}

level_list = []
for k in level_dict.keys():
    level_list.append(k)
level_list.sort()
Start_list = []
End_list = []
Possession_list = []
Kill_list = []
PossessionBullets_list = []
DamageBullets_list = []
PuddleAttack_list = []
Win_list = []
Lose_list = []

for level in level_list:
    total = float(level_dict[level]['Start']) + float(level_dict[level]['End'])
    Start_list.append(round(float(level_dict[level]['Start'])/total, 3))
    End_list.append(round(float(level_dict[level]['End'])/total, 3))
    Possession_list.append(float(level_dict[level]['Possession']))
    Kill_list.append(float(level_dict[level]['Kill']))
    PossessionBullets_list.append(float(level_dict[level]['PossessionBullets']))
    DamageBullets_list.append(float(level_dict[level]['DamageBullets']))
    PuddleAttack_list.append(float(level_dict[level]['PuddleAttack']))
    Win_list.append(float(level_dict[level]['Win']))
    Lose_list.append(float(level_dict[level]['Lose']))

transformation_list = {
    'Start': np.array(Start_list),
    'End': np.array(End_list),
}
width = 0.6  # the width of the bars: can also be len(x) sequence
fig, ax = plt.subplots()
bottom = np.zeros(5)

for key, value in transformation_list.items():
    p = ax.bar(level_list, value, width, label=key, bottom=bottom)
    bottom += value
    ax.bar_label(p, label_type='center')

ax.set_title('Before & After Transformation Percentage')
ax.legend()
plt.savefig('Transformation Percentage Statistics.png', dpi=200)
plt.show()



possession_kill = {
    'Possession': Possession_list,
    'Kill': Kill_list,
}

x = np.arange(len(level_list))  # the label locations
width = 0.25  # the width of the bars
multiplier = 0

fig, ax = plt.subplots(layout='constrained')

for attribute, measurement in possession_kill.items():
    offset = width * multiplier
    rects = ax.bar(x + offset, measurement, width, label=attribute)
    ax.bar_label(rects, padding=3)
    multiplier += 1

# Add some text for labels, title and custom x-axis tick labels, etc.
ax.set_ylabel('Times')
ax.set_title('Possessed & Killed Enemies')
ax.set_xticks(x + width, level_list)
ax.legend(loc='upper left', ncols=3)
ax.set_ylim(0, 250)
plt.savefig('Enemies Statistics.png')
plt.show()



bullets_means = {
    'PossessionBullets': PossessionBullets_list,
    'DamageBullets': DamageBullets_list,
    'PuddleAttack': PuddleAttack_list,
}

x = np.arange(len(level_list))  # the label locations
width = 0.25  # the width of the bars
multiplier = 0

fig, ax = plt.subplots(layout='constrained')

for attribute, measurement in bullets_means.items():
    offset = width * multiplier
    rects = ax.bar(x + offset, measurement, width, label=attribute)
    ax.bar_label(rects, padding=3)
    multiplier += 1

# Add some text for labels, title and custom x-axis tick labels, etc.
ax.set_ylabel('Times')
ax.set_title('Attack Ways Statistics')
ax.set_xticks(x + width, level_list)
ax.legend(loc='upper right', ncols=3)
# ax.set_ylim(0, 250)
plt.savefig('Attack Ways Statistics.png')
plt.show()


# Win-Lost bar chart
fig, ax = plt.subplots()
x = np.arange(len(level_list))  
width = 0.35  

rects1 = ax.bar(x - width/2, Win_list, width, label='Wins')
rects2 = ax.bar(x + width/2, Lose_list, width, label='Loses')

# Add some text for labels, title and custom x-axis tick labels, etc.
ax.set_ylabel('Counts')
ax.set_title('Wins and Loses by Scene')
ax.set_xticks(x)
ax.set_xticklabels(level_list)
ax.legend()

ax.bar_label(rects1, padding=3)
ax.bar_label(rects2, padding=3)

plt.xticks(rotation=45)  # Rotate labels if they overlap
plt.savefig('Wins and Loses Statistics.png')
plt.show()