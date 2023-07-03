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

  * Sword And Shield Slash (For One Handed Light Attack Animation)
  * Sword And Shield Slash (For One Handed Light Attack `Combo` Animation)
  * Sword And Shield Attack (For One Handed Heavy Attack Animation)

  * Sword And Shield Impact (For Damage animation)
  * Sword And Shield Death (For Dead animation)

  * Sword And Shield Casting(For Heal Spell Animation)

  * Great Sword Slash (For Two Handed Light Attack Animation)
  * Great Sword Slash (For Two Handed Light Attack `Combo` Animation)
  * Great Sword Jump Attack (For Two Handed Heavy Attack Animation) 

  * Picking Up (For PickupItem animation)
  * Running Jump (For Jump animation)

  * Zombie Stand Up (For Sleep & WakeUp animations)