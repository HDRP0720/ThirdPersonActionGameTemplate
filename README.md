# Third Person Action Game Template

## Set Up Animations
Both animations need to change Rig from Generic to Humanoid

### For Rolling Animation
* Root Transform Rotation `Bake into Pose` check with Offset -25
* Root Transform Position(Y) `Bake into Pose` check
* Root Transform Position(XZ) `Bake into Pose` uncheck
* animation speed 1.6 ~ 1.8

### For Backstep Animation
* Root Transform Rotation `Bake into Pose` check and change `Based Upon` from Body Orientation to Original
* Root Transform Position(Y) `Bake into Pose` check
* Root Transform Position(XZ) `Bake into Pose` uncheck
* animation speed 1.5

## Mixamo Animations
* Animations
  * Stand To Roll (For Rolling Animation)
  * Standing Dodge Backward (For Backstep Animation)