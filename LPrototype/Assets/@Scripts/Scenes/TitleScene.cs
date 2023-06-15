using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using static Define;

public class TitleScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.TitleScene;
    }

    private void Start()
    {
        GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
        GraphicsSettings.transparencySortAxis = new Vector3(0.0f, 1.0f, 0.0f);

        Managers.Resource.LoadAllAsync<Object>("Preload", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                StartLoaded();
            }
        });
    }

    void StartLoaded()
    {
        Managers.UI.ShowSceneUI<UI_TitleScene>();
    }

    public override void Clear()
    {

    }

}
