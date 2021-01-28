using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Text_Writer : MonoBehaviour {
	
	private static Text_Writer instance;
	private List<Text_Writer_Obj> writerList;
	
	void Awake() {
		instance = this;
        writerList = new List<Text_Writer_Obj>();
	}
	
	// Update is called once per frame
	void Update () {
		if (writerList == null) return;
		for (int i=0; i<writerList.Count; i++) {
			if (writerList[i].Update()) {
                writerList[i].Complete();
				writerList.RemoveAt(i);
				i--;
			}
		}
	}
	
	public static void AddWriter(TextMesh msh, string txt, float speed, bool whitespaces) {
        if (txt == null) txt = "";
		instance.AddWriter_Instance(msh,txt,speed,whitespaces);
	}
	private void AddWriter_Instance(TextMesh msh, string txt, float speed, bool whitespaces) {
		if (writerList == null) {
			writerList = new List<Text_Writer_Obj>();
		}
		writerList.Add(new Text_Writer_Obj(msh,txt,speed,whitespaces));
	}


    public static void ClearWriter(Text uiText) {
        if (instance.writerList != null) {
            for (int i=0; i<instance.writerList.Count; i++) {
                if (instance.writerList[i].GetUiText() == uiText) {
                    instance.writerList.RemoveAt(i);
                    break;
                }
            }
        }
    }
	public static void AddWriter(Text uiText, string txt, float speed, bool whitespaces, bool useDeltaTime = true, Action callbackComplete = null) {
        if (txt == null) txt = "";
		instance.AddWriter_Instance(uiText,txt,speed,whitespaces,useDeltaTime,callbackComplete);
	}
	private void AddWriter_Instance(Text uiText, string txt, float speed, bool whitespaces, bool useDeltaTime = true, Action callbackComplete = null) {
		if (writerList == null) {
			writerList = new List<Text_Writer_Obj>();
		}
		writerList.Add(new Text_Writer_Obj(uiText,txt,speed,whitespaces,useDeltaTime,callbackComplete));
	}
    public static void RemoveWriter(Text uiText) {
        instance.RemoveWriter_Instance(uiText);
    }
    private void RemoveWriter_Instance(Text uiText) {
        if (writerList == null) return;
        for (int i=0; i<writerList.Count; i++) {
            Text_Writer_Obj obj = writerList[i];
            if (obj.GetUiText() == uiText) {
                writerList.RemoveAt(i);
                i--;
            }
        }
    }
}