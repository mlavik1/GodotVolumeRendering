using System.Collections.Generic;
using System;
using Godot;

public class TransferFunction
{
    public List<TFColourControlPoint> colourControlPoints = new List<TFColourControlPoint>();
    public List<TFAlphaControlPoint> alphaControlPoints = new List<TFAlphaControlPoint>();

    private const int TEXTURE_WIDTH = 512;

    private ImageTexture texture = null;

    public void AddControlPoint(TFColourControlPoint ctrlPoint)
    {
        colourControlPoints.Add(ctrlPoint);
    }

    public void AddControlPoint(TFAlphaControlPoint ctrlPoint)
    {
        alphaControlPoints.Add(ctrlPoint);
    }

    public ImageTexture GetTexture()
    {
        if (texture == null)
            GenerateTexture();

        return texture;
    }

    public void GenerateTexture()
    {
        List<TFColourControlPoint> cols = new List<TFColourControlPoint>(colourControlPoints);
        List<TFAlphaControlPoint> alphas = new List<TFAlphaControlPoint>(alphaControlPoints);

        // Sort lists of control points
        cols.Sort((a, b) => (a.dataValue.CompareTo(b.dataValue)));
        alphas.Sort((a, b) => (a.dataValue.CompareTo(b.dataValue)));

        // Add colour points at beginning and end
        if (cols.Count == 0 || cols[cols.Count - 1].dataValue < 1.0f)
            cols.Add(new TFColourControlPoint(1.0f, new Color(0.0f, 0.0f, 0.0f, 0.0f)));
        if (cols[0].dataValue > 0.0f)
            cols.Insert(0, new TFColourControlPoint(0.0f, new Color(0.0f, 0.0f, 0.0f, 0.0f)));

        // Add alpha points at beginning and end
        if (alphas.Count == 0 || alphas[alphas.Count - 1].dataValue < 1.0f)
            alphas.Add(new TFAlphaControlPoint(1.0f, 1.0f));
        if (alphas[0].dataValue > 0.0f)
            alphas.Insert(0, new TFAlphaControlPoint(0.0f, 0.0f));

        int numColours = cols.Count;
        int numAlphas = alphas.Count;
        int iCurrColour = 0;
        int iCurrAlpha = 0;

        float[] tfCols = new float[TEXTURE_WIDTH * 4];

        for (int iX = 0; iX < TEXTURE_WIDTH; iX++)
        {
            float t = iX / (float)(TEXTURE_WIDTH - 1);
            while (iCurrColour < numColours - 2 && cols[iCurrColour + 1].dataValue < t)
                iCurrColour++;
            while (iCurrAlpha < numAlphas - 2 && alphas[iCurrAlpha + 1].dataValue < t)
                iCurrAlpha++;

            TFColourControlPoint leftCol = cols[iCurrColour];
            TFColourControlPoint rightCol = cols[iCurrColour + 1];
            TFAlphaControlPoint leftAlpha = alphas[iCurrAlpha];
            TFAlphaControlPoint rightAlpha = alphas[iCurrAlpha + 1];

            float tCol = (Mathf.Clamp(t, leftCol.dataValue, rightCol.dataValue) - leftCol.dataValue) / (rightCol.dataValue - leftCol.dataValue);
            float tAlpha = (Mathf.Clamp(t, leftAlpha.dataValue, rightAlpha.dataValue) - leftAlpha.dataValue) / (rightAlpha.dataValue - leftAlpha.dataValue);

            //tCol = Mathf.SmoothStep(0.0f, 1.0f, tCol);
            //tAlpha = Mathf.SmoothStep(0.0f, 1.0f, tAlpha);

            Color pixCol = rightCol.colourValue * tCol + leftCol.colourValue * (1.0f - tCol);
            pixCol.A = rightAlpha.alphaValue * tAlpha + leftAlpha.alphaValue * (1.0f - tAlpha);

            int iPixel = iX * 4;
            tfCols[iPixel] = pixCol.R;
            tfCols[iPixel+1] = pixCol.G;
            tfCols[iPixel+2] = pixCol.B;
            tfCols[iPixel+3] = pixCol.A;
        }

        byte[] byteArray = new byte[tfCols.Length * 4];
        Buffer.BlockCopy(tfCols, 0, byteArray, 0, byteArray.Length);
        Image image = Image.CreateFromData(TEXTURE_WIDTH, 1, false, Image.Format.Rgbaf, byteArray);
        texture = ImageTexture.CreateFromImage(image);
    }
}
