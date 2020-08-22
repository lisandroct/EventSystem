# Event System
Event system for Unity using ScriptableObjects based on the concept introduced by Ryan Hipple in his [talk](https://www.youtube.com/watch?v=raQ3iHhE_Kk) at Unite 2017.

## Installation
This package depends on [Core](https://github.com/lisandroct/Core). Both packages can be installed through the Unity Package Manager or by modifying the project packages manifest manually.
```
{
    "dependencies": {
        "com.lisandroct.core": "https://github.com/lisandroct/Core.git",
        "com.lisandroct.events": "https://github.com/lisandroct/EventSystem.git"
    }
}
```

## Usage
After installing the package, the settings can be found in `Edit/Project Settings/EventSystem`.

### Code generation
Before being able to use the Event System, it's necessary to generate the required code for the desired event types. In order to do so, in `Edit/Project Settings/EventSystem/Event Types`, click `Generate` and all the necessary code will be generated automatically for you.

### Adding new event types
Each event can have up to four arguments. You can add new event types in `Edit/Project Settings/EventSystem/Add New`.

If this is your first time using the package, it's recommended to import the Base Settings sample from the Unity Package Manager and replace the settings file located under `lisandroct/Editor/Resources/` with the one provided in the sample.

### Events
After generating the code for each event type, you can create the event assets.

### Listeners
There's nothing to prevent you from subscribing to the events from code but Event System also provides you with Listeners for each event type you generate.

## Example
If this is your first time using the package, it's recommended to import the example from the Unity Package Manager.