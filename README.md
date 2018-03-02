# GridLock
An implementation of a grid based system with pathfinding for Unity3D.

GridLock can be used to manage movement within a grid.

* Grid
  * GridPosition
  * GridBlock
  * GridEntity
  * GridManager

* Pathfinding
  * GridNavAgent
  * GridNode
  * GridPathNode
  * GridNavManager


## Grid

### GridPosition
A representation of a position within the Grid. Think of it as a Vector3 for the grid system.

### GridBlock
Represents a single Block inside the Grid.

### GridEntity
An entity that moves or interacts with the grid system.

### GridManager
Generates and manages the grid.


## Pathfinding

### GridNavAgent
Calculates a path for a single entity based on start point and target. Also stores the path until new one is requested.

### GridNode
Similar to GridBlock but used only for pathfinding.

### GridPathNode
Used for calculating node costs.

### GridNavManager
A duplicated grid with GridNodes instead of Blocks.


