# Third Person Action Game Template

## Set Up Animations
Both animations need to change Rig from Generic to Humanoid

### For Rolling Animation
* Root Transform Rotation `Bake into Pose` check with Offset -25
* Root Transform Position(Y) `Bake into Pose` check
* Root Transform Position(XZ) `Bake into Pose` uncheck
* animation speed 1.6 ~ 1.8
* Add ResetIsInteracting script under the animation node at the animator controller

### For Backstep Animation
* Root Transform Rotation `Bake into Pose` check and change `Based Upon` from Body Orientation to Original
* Root Transform Position(Y) `Bake into Pose` check
* Root Transform Position(XZ) `Bake into Pose` uncheck
* animation speed 1.5
* Add ResetIsInteracting script under the animation node at the animator controller

### For Falling Animation
* Loop Time check
* Loop Pose check
* Root Transform Rotation `Bake into Pose` check
* Root Transform Position(Y) `Bake into Pose` check
* Root Transform Position(XZ) `Bake into Pose` check
* animation speed 2.5

### For Landing Animation
* Change Start frame from 0 to 8
* Root Transform Position(Y) `Bake into Pose` check
* animation speed 1.5
* `Foot IK` check under the animation node at the animator controller

## Mixamo Animations
* Animations
  * Stand To Roll (For Rolling Animation)
  * Standing Dodge Backward (For Backstep Animation)
  * Fall A Loop (For Falling Animation)
  * Falling To Landing (For Landing Animation)