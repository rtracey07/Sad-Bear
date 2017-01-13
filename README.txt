-------------------------------------------------------------------------------
CSCI 4168 - Assignment #1

SAD BEAR

By Robert Tracey 
B00699803
October 13th, 2016
-------------------------------------------------------------------------------
*******************************************************************************

Resources
---------

The following resources were used in the project:

- "Cute Teddy Bear Toy" 
		- from https://www.assetstore.unity3d.com/en/#!/content/50129

- "Mobile Power Ups Free Vol. 1"
		- from https://www.assetstore.unity3d.com/en/#!/content/36106

- "Raw Mocap Data for Mecanim"
		- from https://www.assetstore.unity3d.com/en/#!/content/5330

- "Sample- Low Poly Nature Assets"
		- https://www.assetstore.unity3d.com/en/#!/content/67201

- Font from http://www.1001freefonts.com/


The following video was referenced as a starting point for my BufferedOrb
shader:

- https://www.youtube.com/watch?v=C6lGEgcHbWc


-------------------------------------------------------------------------------
*******************************************************************************

Game Extras
-----------

The following extras were included in the project:

- Custom Shaders:
	- PostEffect image effect shader that pushes all pixels onscreen to 
		grayscale based off of the Colormask alpha value.

	- BufferedOrb shader that, in 3 passes, adds object highlights at intersect
		points by using the camera's depthNormal texture, as well as setting
		the Colormask alpha value.

	- StandardGrayable shader that, in 2 passes, runs the Unity standard pass
	    and then set the Colormask alpha value.


- Blender 3D model:
	- All in-game platforms are modeled by me using Blender. The geometry is
		a modification of a torus.

- Blender Character Rigging:
	- The player and enemy character models I required from the asset store
		did not have rigs, so I imported them into blender and created the 
		skeleton and blend weights for the teddy bears.

- Custom AnimatorControllers and Animations:
	- All of the AnimatorControllers were my own work. All animations other
		than the Mocap data from the Asset Store are also my own work.

-------------------------------------------------------------------------------
