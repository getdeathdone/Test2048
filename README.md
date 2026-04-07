# Test2048 (Unity, 2048 3D)

Implemented according to the assignment:

- Target platform: Android.
- Scene with a physical board and forward cube launching.
- A new cube is spawned at the spawn point at game start and after each launch.
- Spawned cube value:
  - `2` with 75% probability
  - `4` with 25% probability
- Controls:
  - while holding the active cube, it can be moved left/right
  - on release, the cube is launched forward using a physical impulse
- Cube merge happens only if:
  - both cubes have the same value
  - directed collision speed is above the minimum threshold
- Merge result:
  - two cubes become one
  - resulting value is the next power of two (sum of equal values)
- Score system for merges:
  - `2+2 -> +1`
  - `4+4 -> +2`
  - `8+8 -> +4`
- Game Over is implemented using a maximum cubes-on-board limit (`MaxCubesOnBoard`, default: 40).
- Game restart is implemented (`Restart` button) with full state reset.
