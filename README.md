# Gravity Jump

Gravity Jump is a 2D space platformer. You play as Eudes, a friendly alien who jumps from planet to planet to avoid a dangerous black hole.

## Run

To run the game, after build:
- On Windows, in the `gravity_jump` folder, run `GravityJump.exe`
- On macOS: run `gravity_jump.app`

### Multiplayer

When starting the game, go to the `Host` menu.

> If you use Linux, you may need to add a firewall rule allowing the other player to reach your
> local server. The listening port is 8000.

The other player can go to the `Join` menu and enter the host IP in the input field. After clicking `Join`, both players should be redirected to the lobby and be able to start a new game.

See demo in `doc` folder:

[demo](doc/DemoMultiplayer.m4v)

## Build

Requirements:
`Unity 2019.3.12f1`

To build the game from its source code:

- Open `Unity Hub`
- Add new project
- Select `GravityJump` folder in this project directory
- Open the `GravityJump` project in Unity
- Open the build options with `File / Build Settings`
- Select your current OS in `PC, Mac & Linux standalone`
- Build project by clicking on `Build` (or `Build and Run` if you want to play immediately)

## Assets

The source code is located in `GravityJump/Assets`:

- `Fonts` folder contains fonts for the game menu
- `Resources` contains data (game objects, sound, images) that can be instantiated from code at run time
- `Scenes` contains the different game scenes data
- `Scripts` contains the scripts, written in C#
- `Sprites` contains the images that are used to display 2D objects in game

The following section dives into the source code written in `Scripts` folder.

## Code architecture

This section explains the main game classes, defined in the `Scripts` folder.

### Requirements:

All classes are using the [Unity Scripting API](https://docs.unity3d.com/Manual/ScriptingConcepts.html). A basic understanding of this API is required to read through the code.

Most of our classes inherit from `MonoBehaviour`, the base class for Unity scripts. Therefore they already have a base constructor that are called by Unity state machine.

`MonoBehaviour` provide methods such as `Awake` and `Update` that are called at precise moments during game life cycle. Overriding these methods allows to synchronize custom logic with game loop.

> Note: here is more information about the [time management](https://docs.unity3d.com/Manual/TimeFrameManagement.html) and the [life cycle](https://docs.unity3d.com/Manual/ExecutionOrder.html) functions.

### Controllers

`Controllers` namespace contains classes that manage the state of the different scenes. They are responsible for:

- Displaying UI elements
- Managing connection classes (for multiplayer)
- Instantiating game objects (player, planets, bonuses)
- Managing game state (game speed, score, game over)
- Playing music

The different controllers are:

- `BaseController` is an abstract controller that define common behavior for controllers.
- `Menu` is the controller for the Menu scene, which is instantiated first when running the game.
- `Game` is the controller the Game scene, which is the actual game. It is instantiated when starting the game from the Menu (solo or multiplayer) ( see Notes on Game controller).

#### Notes on Game controller

The Game controller handles most of the logic of the game :

On Awake and Start, fields are initialized.

On each Update,

- If needed and possible, the players are instantiated
- If solo or host, handle spawn management
- If multiplayer, listen and handle network messages, send position
- Move the camera and update score
- Handle player’s death if needed

There are also methods to :

- Send player’s position
- Spawn on spawn payload reception
- Handles game over
- Handles going back to menu

### Network

This namespace contains the definition of the network communication interfaces and their implementations. The core of the namespace is the `Connection` class that wraps a `TcpClient` with methods to read and write from and to a `NetworkStream`.

The frames that can be sent over the network must implement the `Payload` interface, and should inherit from the `BasePayload` abstract class. It is defined by an operation code (`OpCode` enumeration).

### Data

This namespace defines a static class `Storage` playing the role of local in-memory storage, allowing scenes to share common data.

### UI

This namespace defines a UI manager for the menu in the game. Especially it defines a state machine as a stack of screens objects that implement the `IGameState` interface. It has four methods, corresponding to the life cycle steps of a menu panel :

- `OnStart` : when the game state is pushed over the screen
- `OnStop` : when the game state is popped from the screen
- `OnPause` : when the game state is covered by another one
- `OnResume`: when the game state is uncovered

Usually these methods toggle the display of the panel elements. This basic behavior is implemented by the `BasicScreen` abstract class. Other behaviors are implemented in child classes defined in the same namespace.

In order to perform comparisons over the screen type with, there is a Name attribute in the `BasicScreen` class, typed with the `UI.Names.Menu` enumeration.

The Unity `GameObjects` elements are get at the instantiation of the classes using the Unity API.

### Physic

`Physic` namespace contains all classes responsible for simulating physics in the game. These classes are located in `Physic` directory.

The namespace contains three main classes:

- `PlayerMovingState` is a state machine representing the player moving state (walking, sprinting, jumping, falling) with an enum. It provides methods to transition between states.
- `AttractableBody` is responsible for applying physics to the player, depending on its `PlayerMovingState`, its game environment and user inputs. It also triggers `PlayerMovingState` updates. It manages the following:

  - Local gravity
  - Orbit switching (when jumping from one planet to another)
  - Walking and sprinting
  - Jumping

- `AttractiveBody`, which provides getters for planet physics.

> Note: in order to ensure a better gameplay, physics computed for the game doesn’t match the real world physics laws.

### Animation

`Animation` namespace provide classes for sprite animation.

- `Animator` is an abstract class defining default behavior for all animators.
- `PlayerAnimator` inherits from `Animator`. It manages the player animation depending on its `PlayerMovingState`.
- `BlackholeAnimator` inherits from `Animator`. It displays the black hole animation.

### Audio

`Audio` name manages the game music and sounds.

`MusicPlayer` loads music from `Resources` folder at runtime and provides methods to play it.

### Players

`Players` namespace contains classes for instantiating and managing players objects in game.

Local player is the actual character controlled by the user. Remote player is the character controlled by the other player in a multiplayer game session.
Spawner is an abstract class that define common default behavior for the player spawners.

LocalPlayerSpawner inherits from Spawner. It is responsible for instantiating the local player near a planet and triggering physics on the player depending on user actions (walk, jump and sprint).

RemotePlayerSpawner inherits from Spawner. In multiplayer mode, it is responsible for instantiating the remote player and moving it depending on the positions sent by the other player game.

### Background

`Background` namespace provide classes for planets.

`Manager` manages the position of background tiles (see Notes on Perspective).

#### Notes on Perspective

In the game, an orthographic projection is used. It means that the farther (z position) an object is from the camera (at z = -10), the smaller it appears on camera.

At z=0, the field of view is within -5 and 5 along the y axis, and between -10 and 10 along the x axis.

As a consequence, the field of view depends on the z position. This property is important for the game’s background.

Our three layers of background follow a parallax effect.

Their size is expanded four time along the y axis, and their sprite is tiled.

Periodically, they translate for the actual sprite’s width, so that the player does not see the difference, and experience an infinite scrolling background.

### Decors

`Decors` namespace provide classes for collectibles.

- `Decor` is an abstract class defining default behavior for all collectibles.
- `ShootingStar` inherits from `Decor`. It implements a shooting star decor.
- `Corpse` inherits from `Decor`. It implements a corpse decor.
- `Spawner` inherits from `ObjectManagement`. It spawn Decors at random locations.

Players and planets cannot interact in any way with the decors.

### Collectibles

`Collectibles` namespace provide classes for collectibles.

- `Collectible` is an abstract class defining default behavior for all collectibles.
- `Boost` inherits from `Collectible` (see Spawners section). It implements the OnCollect method for boosters, pushing the player away from the black hole.
- `Minimizer` inherits from `Collectible`. It implements the OnCollect method for minimizers and maximizers, making the player bigger, or smaller.
- `Spawner` inherits from `ObjectManagement` (see Spawners section). It spawn Collectibles at random location. If they spawn on planets, they are destroyed.

Collectibles are little object that can be collected by the player during the run. It can help (or not) the player to escape the black hole.

### Planets

`Planets` namespace provide classes for planets.

- `SpawningPoint` is used to store the initial position of the player, on the first planet when starting a run.
- `Spawner` inherits from `ObjectManagement` (see Spawners section). It spawn Decors at random locations.

### ObjectManagement

`ObjectManagement` namespace provide classes to handle objects .

- `Destroyer` situated behind the black hole, destroy instances of object to maintain a constant number of instantiated objects.
- `Spawner` is an abstract class defining default behavior for all spawners (see Notes on Spawners section).

#### Notes on Spawners

Spawners are objects, following the camera, situated further on the right, out of the view.

They instantiate objects :

- In multiplayer, as a client, when a Spawn message is received
- As host, or in solo mode, iteratively : the following spawn settings are computed after each spawn.

There are three types of spawners inheriting the ObjectManagement.Spawner class :

- `Planets.Spawner`, for planets (Attractive bodies) : Position and size are very important for both players
- `Collectibles.Spawner`, for collectibles : Position is very important.
- `Decors.Spawner`, for decors (non interactive objects) : Position, and other settings are randomly generated on each end.

The Player spawner has different properties and methods, and is only used to instantiate the player on start.

## Multiplayer architecture

The networking part is supported by a peer to peer architecture using TCP.

The `Host` is running a local server and wait for a specific payload from another client wanting to join its game. Once a connection is established, a network stream is shared by both players. They will exchange their positions over this channel.

The `Host` is the source of truth for the randomly generated objects of the game, such as planets, collectibles and visual effects. After having locally generated them, it serializes them and send them to the other player which deserializes them and instantiates them accordingly.

Finally a player that loses the game by being reached by the black hole sends a message to the other one, warning it the game is over.

## Credits

### Design

Clara Clement

### Music

Quentin Verlhac

### Development

Julien Doutre

Hugo Saint-Vignes

Quentin Verlhac
