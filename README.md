# ICIS_UNITY
Evolutionary algorithm project using Unity.

![Documentation](https://github.com/iulian-b/ICS-Project/blob/master/Documentation.pdf)

# Setup
a. Download the source and import the root folder as a project into Unity version>2021.3 (earlier builds have not been tested)

or

b. Download the _.unitypackage_ from [releases](https://github.com/iulian-b/ICS-Project/releases/) and open it, importing everything

# Instructions
Alter the settings from the main menu if needed and press the **[LOAD]** button.

To reset the settings to default, press the **[RESET]** button.

Finally, press **[START]**.

To alter the behaviour or the implementation of the EA, modify the _GameLogic.cs_ script. The other scripts should be left alone.

# Export
If the **DUMPDATA** option has been checked from the main menu before running the main scene, the program will write to logs about each generation and Y-asxis plot points for the best and average fitness values from each run to the directory. These logs are found inside _/Assets/DataDump/_.

## Plot
To plot a graph, move _plot_avg_ and _plot_bst_ from _/Assets/DumpData/_ to the _/Assets/Plot/_ directory, and modify/run the _plot.py_ script to generate a graph with the results. 

---
# Credits
Car 3D mesh by [edwinamadormeza](https://www.cgtrader.com/free-3d-models/car/racing-car/red-bull-rb6-2010)

Monza 3D mesh by [Ryan Brands](https://grabcad.com/library/monza-gp-model-1) (mesh was slightly modified by me)
