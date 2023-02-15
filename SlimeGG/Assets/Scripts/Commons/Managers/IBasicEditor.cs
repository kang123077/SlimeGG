using UnityEngine;

public interface IBasicEditor
{
    public void onClickStart(Transform clickedTf, Vector3 clickedPos);
    public void onClickLeftInPlace(Transform clickedTf);

    public void onClickRightInPlace(Transform clickedTf);
    public void onDragLeft(Vector3 clickedPos);
    public void onDragRight(Vector3 clickedPos);
    public void onClickLeftEnd(Transform clickedTf, Vector3 clickedPos);
    public void onClickRightEnd(Transform clickedTf, Vector3 clickedPos);
    public void onClickStop(Vector3 lastMousePos);
    public void saveIntoFile(string fileName, string displayName);
    public void loadFromFile(string fileName);
    public void clearAll();
}