using UnityEngine.EventSystems;

public static class UIUtil
{
    public static bool IsClickOverUI()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
