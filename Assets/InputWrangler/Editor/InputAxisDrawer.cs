using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(InputAxis))]
public class InputAxisDrawer : PropertyDrawer
{
	static List<string> inputNames = new List<string>();

	static InputAxisDrawer()
	{
		RefreshInputs();
	}

	static void RefreshInputs()
	{

		Object manager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
		SerializedObject obj = new SerializedObject(manager);
		SerializedProperty axisArr = obj.FindProperty("m_Axes");

		inputNames.Clear();

		for (int i = 0; i < axisArr.arraySize; i++)
		{
			SerializedProperty entry = axisArr.GetArrayElementAtIndex(i);
			string name = GetChild(entry, "m_Name").stringValue;
			if (inputNames.Contains(name))
				continue;
			else
				inputNames.Add(name);
		}
	}

	static SerializedProperty GetChild(SerializedProperty p, string name)
	{
		SerializedProperty child = p.Copy();
		child.Next(true);

		if (child.name == name)
			return child;

		while (child.Next(false))
		{
			if (child.name == name)
			{
				return child;
			}
		}

		return null;
	}

	static void OpenInputManager()
	{
		Object manager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
		Selection.activeObject = manager;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		if (property.propertyType != SerializedPropertyType.String)
		{
			EditorGUI.LabelField(position, "ERROR:", "May only apply to type string");
			return;
		}

		position = EditorGUI.PrefixLabel(position, label);

		string name = property.stringValue;
		Color previousColor = GUI.color;
		if (name == "")
			GUI.color = Color.grey;
		else if (!inputNames.Contains(name))
			GUI.color = Color.red;

		if (GUI.Button(position, name == "" ? "<None>" : name, EditorStyles.popup))
		{
			Selector(property);
		}
		GUI.color = previousColor;

	}

	SerializedProperty currentProperty;
	void Selector(SerializedProperty property)
	{
		currentProperty = property;
		GenericMenu menu = new GenericMenu();

		menu.AddItem(new GUIContent("Refresh"), false, RefreshInputs);
		menu.AddItem(new GUIContent("InputManager"), false, OpenInputManager);
		menu.AddSeparator("");
		menu.AddItem(new GUIContent("<None>"), property.stringValue == "", HandleSelect, "");
		for (int i = 0; i < inputNames.Count; i++)
		{
			string name = inputNames[i];
			menu.AddItem(new GUIContent(name), property.stringValue == name, HandleSelect, name);
		}


		menu.ShowAsContext();
	}

	void HandleSelect(object data)
	{
		currentProperty.stringValue = (string)data;
		currentProperty.serializedObject.ApplyModifiedProperties();
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return 18;
	}
}
