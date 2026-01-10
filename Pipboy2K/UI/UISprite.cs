using System.Numerics;
using Raylib_cs;

namespace Pipboy2K.UI;

public class UISprite : UIWidget
{
    private int spriteResX = 0;
    private int spriteResY = 0;

    private Texture2D? spriteTexture;

    private Rectangle sourceRect;

    public UISprite(Texture2D texture, int spriteResX, int spriteResY)
    {
        this.spriteTexture = texture;

        this.spriteResX = spriteResX;
        this.spriteResY = spriteResY;

        SetFrame(0);

    }

    public override void Paint()
    {
        if (spriteTexture == null)
            return;

        DrawTextureRec(spriteTexture.Value, sourceRect, Vector2.Zero, Color.White);
    }

    public void SetFrame(int i)
    {
        sourceRect = new Rectangle(spriteResX * i, 0, spriteResX, spriteResY);
    }

    public void SetTexture(Texture2D texture) => spriteTexture = texture;
}