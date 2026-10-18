
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class RescueStarEffect : MonoBehaviour
{
    public Sprite starSprite;

    private ParticleSystem ps;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();

        if (starSprite == null)
        {
            Debug.LogWarning("Star Sprite is not assigned!");
            return;
        }

        ParticleSystemRenderer psRenderer =
            GetComponent<ParticleSystemRenderer>();

        Material mat = psRenderer.material;

        if (mat.HasProperty("_BaseMap"))
        {
            mat.SetTexture("_BaseMap", starSprite.texture);
        }

        if (mat.HasProperty("_MainTex"))
        {
            mat.SetTexture("_MainTex", starSprite.texture);
        }

        var textureSheet = ps.textureSheetAnimation;
        textureSheet.enabled = true;
        textureSheet.mode = ParticleSystemAnimationMode.Sprites;
        textureSheet.SetSprite(0, starSprite);
    }
}
