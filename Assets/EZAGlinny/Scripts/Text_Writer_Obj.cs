using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Text_Writer_Obj {
	
	private TextMesh msh;
    private Text uiText;
	private string txt;
	private float timer;
	private float speed;
	private int txtIndex;
	private bool whitespaces;
    private bool useDeltaTime;
    private Action callbackComplete;
	
	public Text_Writer_Obj(TextMesh _msh, string _txt, float _speed, bool _whitespaces) {
		msh = _msh;
		txt = _txt;
		speed = _speed;
		whitespaces = _whitespaces;
        useDeltaTime = false;
		
        if (txt == null || (txt != null && txt == "")) txt = " ";
		timer = 0f;
		txtIndex = 1;
	}
	public Text_Writer_Obj(Text _uiText, string _txt, float _speed, bool _whitespaces, bool useDeltaTime = true, Action callbackComplete = null) {
		uiText = _uiText;
		txt = _txt;
		speed = _speed;
		whitespaces = _whitespaces;
        this.callbackComplete = callbackComplete;
        this.useDeltaTime = useDeltaTime;
		
        if (txt == null || (txt != null && txt == "")) txt = " ";
		timer = 0f;
		txtIndex = 1;
	}
    public Text GetUiText() {
        return uiText;
    }
    public void Complete() {
        if (callbackComplete != null)
            callbackComplete();
    }
	public bool Update() {
		//Returns true when finished
        if (txt == null) return true;

        if (useDeltaTime) {
            float add = Time.deltaTime;
            if (add > .1f) add = .1f;
		    timer += add;
        } else {
            float add = Time.unscaledDeltaTime;
            if (add > .1f) add = .1f;
		    timer += add;
        }

		while (timer >= speed) {
			//Write next letter
			string text = txt.Substring(0,txtIndex);
			if (whitespaces) {
                text += "<color=#00000000>"+txt.Substring(txtIndex)+"</color>";
				/*for (int i=txtIndex; i<txt.Length; i++)
					text += "\u0020";
				text += ".";*/
			}
            if (msh != null)
			    msh.text = text;
            if (uiText != null)
                uiText.text = text;
			txtIndex++;
			timer -= speed;
            if (txtIndex > txt.Length) break;
		}
		return txtIndex > txt.Length;
	}
}