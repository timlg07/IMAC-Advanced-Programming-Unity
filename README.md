# Advanced programming - Unity

## Controls
Unity Free Camera (move with arrow keys/WASD, look around while pressing the right mouse button)

## Objectives
- Learn to develop & increase XP in an Unity3D environment
- Perform some backward-engineering (start by copying features)
- Understand usage of pathfinding algorithms, tools, and documentation

## Tools
Minimal toolset to work with is:
- Unity
	- Official target version: 2021.1.2f1
	- Please note that many versions will actually work, from 2018.1 and forth
- Navigation AI package
	- >= 2022.2: https://docs.unity3d.com/Packages/com.unity.ai.navigation@1.1/manual/index.html
	- >= 2019.4: https://docs.unity3d.com/Packages/com.unity.ai.navigation@1.0/manual/index.html
	- <= 2020.x: Can also be functional with "[Navmesh components](https://github.com/Unity-Technologies/NavMeshComponents.git)" (predecessor of the package)
	- Please note the original *Nav mesh* from "vanilla" Unity is incomplete, and deprecated.

## Features and constraints

### Mandatory
You have to build a "theme park" with visitors walking between points of interest.
1. POIs, with editable variables:
	- `float`: Visit time
	- `position`: entrance, exit
	- `int` \[1;+inf\[ maximum simultaneous visitors
1. Static obstacles: POIs, one large lake
	- Use a "Unity Terrain" as a base
1. Slopes
	- Very high slopes are to be considered "not walkable" (~= "obstacles")
1. Queues, naturally snaking
1. Avoiding other visitors in a more-or-less realistic way, especially in queues
1. Support for charge (> 100 agents)
1. No "Visual Scripting" ("Bolt" / "Blueprint")

### Optional / secondary
1. Bridge: mesh-based additional walkable zone, connected to the terrain
1. Non-visiting people wandering around ("crowd")
1. Slowdown zone around the lake (secondary walkable zone)
1. Additional obstacles & decor: trees, rocks, other lakes/rivers, etc.
1. Other features and aesthetic elements, such as textures, water specular reflections, terrain design, bridges, overall rendering tweaks and effects, camera management and moving, etc.

## Evaluation
Criteria on which your work will be evaluated:
- General **behavior**: Visitors and crown must be credible, and somewhat realistic.
	- Visiting
	- Waiting before entering, queuing
	- Agents avoiding behavior: (~ 1-2 m, except in queues)
	- etc.
- **Charge** test: An in-game command (button, shortcut...) must allow adding agents.
	- Recommended & easy setup: One "*+25 visitors*" button, and one "*+25 crowd*" button (with basic UI).
- **Queuing**: Queue should form itself while characters approach and agglomerate. Shape of the queue is thus undetermined and should change over time when visitors advance one step.
	- If you fail at it, it will be tolerated to have a straight and basic queue.
- Camera: Can be whatever (even static), as long as evaluating other points is still possible.
- Ease of use as a level designer.
- Code quality.
- Secondary objectives and their quality.
	- Please note that graphics is a secondary objective: a solid-colors scene with boxes and a terrain is enough as a primary objective.
