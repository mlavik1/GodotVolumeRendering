using Godot;

public class TransferFunctionDatabase
{
    public static TransferFunction CreateTransferFunction()
    {
        TransferFunction tf = new TransferFunction();
        tf.AddControlPoint(new TFColourControlPoint(0.0f, new Color(0.11f, 0.14f, 0.13f, 1.0f)));
        tf.AddControlPoint(new TFColourControlPoint(0.2415f, new Color(0.469f, 0.354f, 0.223f, 1.0f)));
        tf.AddControlPoint(new TFColourControlPoint(0.3253f, new Color(1.0f, 1.0f, 1.0f, 1.0f)));

        tf.AddControlPoint(new TFAlphaControlPoint(0.0f, 0.0f));
        tf.AddControlPoint(new TFAlphaControlPoint(0.1787f, 0.0f));
        tf.AddControlPoint(new TFAlphaControlPoint(0.2f, 0.024f));
        tf.AddControlPoint(new TFAlphaControlPoint(0.28f, 0.03f));
        tf.AddControlPoint(new TFAlphaControlPoint(0.4f, 0.546f));
        tf.AddControlPoint(new TFAlphaControlPoint(0.547f, 0.5266f));

        tf.GenerateTexture();
        return tf;
    }
}
