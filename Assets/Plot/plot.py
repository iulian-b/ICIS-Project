import matplotlib.pyplot as plt

def Average(lst): 
    return sum(lst) / len(lst) 

yAVG20r = []
yBST20r = []
yAVG40r = []
yBST40r = []
tgtX = []
tgtY = []

Favg20Rand = open('AVRG_100_20_rand_mut.txt', 'r')
Fbst20Rand = open('BEST_100_20_rand_mut.txt', 'r')
Favg40Rand = open('AVRG_100_40_rand_mut.txt', 'r')
Fbst40Rand = open('BEST_100_40_rand_mut.txt', 'r')

for line in Favg20Rand.readlines():
    yAVG20r.append(int(line))
for line in Fbst20Rand.readlines():
    yBST20r.append(int(line))
for line in Favg40Rand.readlines():
    yAVG40r.append(int(line))
for line in Fbst40Rand.readlines():
    yBST40r.append(int(line))

Favg20Rand.close()
Fbst20Rand.close()
Favg40Rand.close()
Fbst40Rand.close()

GEN = len(yAVG20r)
for i in range(0, GEN):
    tgtX.append(i)
for i in range(0, GEN):
    tgtY.append(75)

print(Average(yBST20r))
print(Average(yBST40r))

plt.plot(yAVG20r, '-', label = "β avg_fitness")
plt.plot(yBST20r, '-', label = "β bst_fitness")

plt.plot(yAVG40r, '-', label = "α avg_fitness")
plt.plot(yBST40r, '-', label = "α bst_fitness")

plt.plot(tgtX, tgtY, 'k:', label = "fitness target")

plt.xlabel('Generatii')
plt.ylabel('Fitness')
plt.title('Rezultate dupa ' + str(GEN - 1) + ' de generatii')

plt.legend()
plt.show()