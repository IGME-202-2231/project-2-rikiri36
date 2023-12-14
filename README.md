# Project _NAME_


_REPLACE OR REMOVE EVERYTING BETWEEN "\_"_

### Student Info

-   Name: _Ricky Yu_
-   Section: _02_

## Simulation Design

_In a huge "room" (The whole screen) filled with people/cells wandering around. Able to put down an "infected" person/cell or "infect" one.
   The infection may spread. There will be "healers" around to fight it.[^1]_

### Controls

-   _Drop in an "infect" person/cell_
    -   _Mouse input_
    -   _The infected will chase uninfected, slowly converting all the uninfected if untreated_

-   _UI input (restart)[^2]_
    -   _Ui input_
    -   _restart the simulation_

## _Agent 1 (Uninfected)_

_The main object on the screen. Will "wander" aimlessly. Will run from infected._

### _Wander state_

**Objective:** _The purpose is to wander like they have something to do. (maybe be at a station[^2])_

#### Steering Behaviors

- _Wander_
   - _how many seconds in the future_
   - _wander around. (maybe "do" things at a "station")_
- Obstacles - _Maybe some trees, dead bodies, or other uninfected (agent 1)_
- Seperation - _infected agent[^3]_
   
#### State Transistions

- _All ways agent can transition to this state:_
   - _When this agent sees no infected (agent 2)_

### _Evasion (and flee) state_

**Objective:** _Evade the infected (agent2)._

#### Steering Behaviors

- _evade_
  - _list of target (infected Agent 2)_
  - _Evade agent 2 (infected)_
- Obstacles - _maybe some trees or something, dead bodies_
- Seperation - _agent 2[^3]_
   
#### State Transistions

- _All ways agent can transition to this state:_
  - _When there is an infected (agent 2)_
  - _When is in range  of infected (agent 2)_

## _Agent 2 (Infected)_

_The enemy of the simulation. Will chase afterward nearest (if possible) uninfected. May be defeated by "healers"[^2]_

### _Pursuit ( and seek)_

**Objective:** _Chase after the uninfected (agent 1) and "kill" them._

#### Steering Behaviors

- _pursue_
  - _List of uninfected (agent 1)_
  - _Chase after uninfected (agent 1)_
- Obstacles - _healers, trees or something_
- Seperation - _Agent 1[^3]_
   
#### State Transistions

- _All ways agent can transition to this state:_
  - _When 1 or more uninfected (agent 1) exists_
   
### _Evasion_

**Objective:** _Evade healers[^4]._

#### Steering Behaviors

- _Evade_
  - _List of healers, if any_
  - _Get away from healers (maybe weighted)_
- Obstacles - _Terrarins[^2]_
- Seperation - _Agent 1[^3]_
   
#### State Transistions

- _All ways agent can transition to this state:_
  - _Within range of healers_

## Sources

-   _List all project sources here –models, textures, sound clips, assets, etc._
-   _I am going to hand draw my sprites, models, textures._
    - _Clip studio paint and Pixilart (https://www.pixilart.com/)

## Make it Your Own

- _List out what you added to your game to make it different for you:_
  - _Maybe some extra controls for spawning more enemies or healers

- _If you will add more agents or states make sure to list here and add it to the documention above:_
  - _Maybe a healer. [^4]

- _I will be "hand drawing" all assets for sprites and background.

## Known Issues

_"Game" can be played normally. No known issues._

### Requirements not completed

- _4 unqiue states may not have been implemented. _
- _"Healers", as mentioned in this document, has not been implemented as planned_
- _The uninfected(not green color) does not run from the infected(green)


[^1]: May or may not change. I'm not too sure. Maybe.
[^2]: Maybe. I'm not too sure at this point.
[^3]: I'm not sure if I'm doing this correctly
[^4]: Healer. I was thinking about making agent 1 have a bool for seperating between npc or healers.