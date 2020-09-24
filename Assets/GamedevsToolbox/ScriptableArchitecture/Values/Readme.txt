To create a scriptable value, right click on any folder of the project inside the Unity Editor: Create -> Scriptable Architecture -> Values -> The value you desire.
Set any initial value you want to the scriptable value. Set a default if you are going to reset it anytime.

To support new data types, inherit from GamedevsToolbox.ScriptableArchitecture.Values.ScriptableValue<YourValueType> and set this anotation on your class:
[CreateAssetMenu(menuName = "Scriptable Architecture/Values/TypeName")]

You can implement any extra methods you want in that class, like increment if it is a numeric value.