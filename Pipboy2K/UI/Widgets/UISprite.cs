using System.Numerics;
using Raylib_cs;

namespace Pipboy2K.UI.Widgets;

public class UISprite : UIWidget
{
    private int spriteResX = 0;
    private int spriteResY = 0;

    private Texture2D? spriteTexture;

    private Rectangle sourceRect;

    public int Frames = 8;
    public float FrameStep = 0.1f;

    private int currentFrame = 0;
    private float currentTime = 0.0f;

    Rectangle _destRect;

    public UISprite(Texture2D texture, int spriteResX, int spriteResY)
    {
        this.spriteResX = spriteResX;
        this.spriteResY = spriteResY;


        SetTexture(texture);
    }

    public void Tick()
    {
        currentTime += Raylib.GetFrameTime();

        if (currentTime >= FrameStep)
        {
            currentTime = 0.0f;
            currentFrame += 1;

            if (currentFrame >= Frames)
            {
                currentFrame = 0;
            }
        }

        _destRect.X = spriteResX * currentFrame;
    }


    protected override void Draw(WidgetRenderer e, RenderTransform transform)
    {
        if (spriteTexture == null)
            return;

        e.DrawTextureRec(spriteTexture.Value, _destRect, Vector2.Zero, Color.White);
    }

    public void SetFrame(int i)
    {
        sourceRect = new Rectangle(spriteResX * i, 0, spriteResX, spriteResY);
    }

    public void SetTexture(Texture2D texture)
    {
        spriteTexture = texture;

        _destRect = new Rectangle(0, 0, spriteResX, spriteResY);
    }
}