Use ScriptableReference objects like ScriptableFloatReference or ScriptableStringReference to power up your inspector.

Press the circle on the property to choose between Constant and Value. Use Constant to get the same effect as the type. Use Value to reference a ScriptableValue object.

To extend ScriptableReference use the corresponding type as the first template parameter, and the corresponding ScriptableValue type on the second parameter. You need also to extend ScriptableReferenceDrawer to show the property as the other ScriptableReference.