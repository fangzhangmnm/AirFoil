AirFoil

Aerodynamic Simulation Package

author: fangzhangmnm

Demo Scene:
	Open AirFoil/Demo/Air.scene
	Enable the aircraft you wish to play
	Mouse to control roll and pitch. The mouse input is relative to screen center, so you should put the cursor to the screen center.
	[W] and [S] control the engine, [A] and [D] control yaw.

AirFoil Script:
	Put it to any gameobject you wish to become aerodynamic.
	Copy the Lift, Drag Coefficient Curve from any aircraft prefab in AirFoil/DemoPrefabs/
	You may input the geometry shape by hand. Chord Length is the size in z direction, Section Length is the size in x direction. size in y direction doesn't matter.
	You can also assign a box collider to the Shape Reference Collider, then it will automatically calculate the Chord and Section Length
	
	[WARNING] there should be no scaling in the gameobject and parents the AirFoil Script attached to. See the aircraft prefabs for an example. 
	
	Remember: airflow comes from positive z axis. the aerodynamic force are induced on the y-z plane. airflow and force on x axis are neglected in this model.

Thruster Script:
	It generates a thrust force along the positive z axis. force=value*maxThrust
	
AircraftControlSimple:
	Assign it to the root gameobject of the aircraft. Remember to add a rigidbody to the aircraft. Assign the ailerons (small wings which can be controlled by the player), and the thruster, and the rigidbody.

Tricks in designing an aircraft:
	You may tick the Axes To Center box in the Aircraft Control Simple Script. Then press 10 times [W] to use the maximum thrust. A well designed aircraft should take off and stablize itself without mouse input.
	
	- Take reference pictures of aircrafts and model aircrafts.
	- Max Thrust should be greater than 0.4 times of mass*9.81.
	- Bigger main wings requires lesser speed to take off. For model planes, mass=0.1kg, the surface area (Chord Length * Section Length) might be 0.05m^2.
	- Bigger tails (Both Horizontal and Vertical) are better for stabilizating the aircraft.
	- The center of mass should slightly beneath and under the main wings. It can also be above the main wings if you follow the next trick.
	- Rotate the main wings in z axis to form a V shape. It will decrease the rolling instability.
	- Slightly Rotate the main wings (say, 1-4 degree) in X axis to get a lifting force. You can also not rotate the main wings but use a rotated smaller wing to make the main wings raised when flying. 
	- Adjust the surface area and arm-of-force to get the desired control torque.
	- Remember to add some wheels with small friction. the collider of wheels should not be too small, otherwise the physics engine should go wrong.
	- If the plane started to shake, it may not be bugs of the simulation. It may be the instability of the plane design itself!
	
	
	