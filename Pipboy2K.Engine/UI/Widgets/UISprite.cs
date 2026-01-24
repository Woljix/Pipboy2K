using System.Numerics;
using Raylib_cs;

namespace Pipboy2K.Engine.UI.Widgets;

public class UISprite : UIWidget
{
    private int spriteResX = 0;
    private int spriteResY = 0;

    private Texture2D? spriteTexture;

    private Rectangle sourceRect;

    public int Frames = 8;
    public float FrameStep = 0.1f;

    public bool IsAnimated = false;

    private int currentFrame = 0;
    private float currentTime = 0.0f;

    Rectangle _destRect;

    public UISprite()
    {

    }

    public UISprite(Texture2D texture, int spriteResX, int spriteResY)
    {
        this.spriteResX = spriteResX;
        this.spriteResY = spriteResY;

        SetTexture(texture, this.spriteResX, this.spriteResY);

        if (!IsAnimated)
        {
            _destRect = new Rectangle(0, 0, spriteResX, spriteResY);
        }
    }

    public override void Initialize()
    {
        //this.spriteResX = spriteResX;
        //this.spriteResY = spriteResY;


        //SetTexture(texture);

    }

    public override void Update()
    {
        if (IsAnimated)
            Tick();
    }

    private void Tick()
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

        e.DrawTexture(spriteTexture.Value, _destRect, Vector2.Zero, Color.White);
        //e.DrawTexturePro(spriteTexture.Value, _destRect, )
    }

    public void SetFrame(int i)
    {
        sourceRect = new Rectangle(spriteResX * i, 0, spriteResX, spriteResY);
    }

    public void SetTexture(Texture2D texture, float spriteResX, float spriteResY)
    {
        spriteTexture = texture;

        _destRect = new Rectangle(0, 0, spriteResX, spriteResY);
    }

    public void SetAnimated()
    {
        IsAnimated = true;
    }

    public void SetStatic()
    {

    }
}