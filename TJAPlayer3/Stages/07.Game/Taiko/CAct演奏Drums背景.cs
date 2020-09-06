﻿


using System;
using FDK;

namespace TJAPlayer3
{
    internal class CAct演奏Drums背景 : CActivity
    {
        // 本家っぽい背景を表示させるメソッド。
        //
        // 拡張性とかないんで。はい、ヨロシクゥ!
        //
        public CAct演奏Drums背景()
        {
            base.b活性化してない = true;
        }

        public void tFadeIn(int player)
        {
            this.ct上背景クリアインタイマー[player] = new CCounter(0, 100, 2, TJAPlayer3.Timer);
            this.eFadeMode = EFIFOモード.フェードイン;
        }

        //public void tFadeOut(int player)
        //{
        //    this.ct上背景フェードタイマー[player] = new CCounter( 0, 100, 6, CDTXMania.Timer );
        //    this.eFadeMode = EFIFOモード.フェードアウト;
        //}

        public void ClearIn(int player)
        {
            this.ct上背景クリアインタイマー[player] = new CCounter(0, 100, 2, TJAPlayer3.Timer);
            this.ct上背景クリアインタイマー[player].n現在の値 = 0;
            this.ct上背景FIFOタイマー = new CCounter(0, 100, 2, TJAPlayer3.Timer);
            this.ct上背景FIFOタイマー.n現在の値 = 0;
        }

        public override void On活性化()
        {
            base.On活性化();
        }

        public override void On非活性化()
        {
            TJAPlayer3.t安全にDisposeする(ref this.ct上背景FIFOタイマー);
            for (int i = 0; i < 2; i++)
            {
                ct上背景スクロール用タイマー1st[i] = null;
                ct上背景スクロール用タイマー2nd[i] = null;
                ct上背景スクロール用タイマー3rd[i] = null;
            }
            TJAPlayer3.t安全にDisposeする(ref this.ct下背景スクロール用タイマー1);
            base.On非活性化();
        }

        public override void OnManagedリソースの作成()
        {
            this.ct上背景スクロール用タイマー1st = new CCounter[2];
            this.ct上背景スクロール用タイマー2nd = new CCounter[2];
            this.ct上背景スクロール用タイマー3rd = new CCounter[2];
            this.ct上背景クリアインタイマー = new CCounter[2];

            for (int i = 0; i < 2; i++)
            {
                if (TJAPlayer3.Tx.Background_Up_3rd[i] != null)
                {
                    this.ct上背景スクロール用タイマー1st[i] = new CCounter(1, TJAPlayer3.Tx.Background_Up_1st[i].szテクスチャサイズ.Width, 16, TJAPlayer3.Timer);
                    this.ct上背景スクロール用タイマー2nd[i] = new CCounter(1, TJAPlayer3.Tx.Background_Up_2nd[i].szテクスチャサイズ.Height, 70, TJAPlayer3.Timer);
                    this.ct上背景スクロール用タイマー3rd[i] = new CCounter(1, 120, 1000 / 60, TJAPlayer3.Timer);
                    this.ct上背景クリアインタイマー[i] = new CCounter();
                }
            }

            if (TJAPlayer3.Tx.Background_Down_Scroll != null)
                this.ct下背景スクロール用タイマー1 = new CCounter(1, TJAPlayer3.Tx.Background_Down_Scroll.szテクスチャサイズ.Width, 4, TJAPlayer3.Timer);

            this.ct上背景FIFOタイマー = new CCounter();
            base.OnManagedリソースの作成();
        }

        public override void OnManagedリソースの解放()
        {
            //CDTXMania.tテクスチャの解放( ref this.tx上背景メイン );
            //CDTXMania.tテクスチャの解放( ref this.tx上背景クリアメイン );
            //CDTXMania.tテクスチャの解放( ref this.tx下背景メイン );
            //CDTXMania.tテクスチャの解放( ref this.tx下背景クリアメイン );
            //CDTXMania.tテクスチャの解放( ref this.tx下背景クリアサブ1 );
            //Trace.TraceInformation("CActDrums背景 リソースの開放");
            base.OnManagedリソースの解放();
        }

        public override int On進行描画()
        {
            this.ct上背景FIFOタイマー.t進行();

            for (int i = 0; i < 2; i++)
            {
                if (this.ct上背景クリアインタイマー[i] != null)
                    this.ct上背景クリアインタイマー[i].t進行();
            }
            for (int i = 0; i < 2; i++)
            {
                if (this.ct上背景スクロール用タイマー1st[i] != null)
                    this.ct上背景スクロール用タイマー1st[i].t進行Loop();
            }
            for (int i = 0; i < 2; i++)
            {
                if (this.ct上背景スクロール用タイマー2nd[i] != null)
                    this.ct上背景スクロール用タイマー2nd[i].t進行Loop();
            }
            for (int i = 0; i < 2; i++)
            {
                if (this.ct上背景スクロール用タイマー3rd[i] != null)
                    this.ct上背景スクロール用タイマー3rd[i].t進行Loop();
            }
            if (this.ct下背景スクロール用タイマー1 != null)
                this.ct下背景スクロール用タイマー1.t進行Loop();



            #region 1P-2P-上背景
            for (int i = 0; i < 2; i++)
            {
                if (this.ct上背景スクロール用タイマー1st[i] != null)
                {
                    double TexSizeL = 1280 / TJAPlayer3.Tx.Background_Up_1st[i].szテクスチャサイズ.Width;
                    double TexSizeW = 308 / TJAPlayer3.Tx.Background_Up_2nd[i].szテクスチャサイズ.Height;
                    double TexSizeF = 1280 / TJAPlayer3.Tx.Background_Up_3rd[i].szテクスチャサイズ.Width;
                    // 1280をテクスチャサイズで割ったものを切り上げて、プラス+1足す。
                    int ForLoopL = (int)Math.Ceiling(TexSizeL) + 1;
                    int ForLoopW = (int)Math.Ceiling(TexSizeW) + 1;
                    int ForLoopF = (int)Math.Ceiling(TexSizeF) + 1;
                    //int nループ幅 = 328;

                    #region [ 上背景-Back ]

                    for (int W = 1; W < ForLoopW + 1; W++)
                    {
                        TJAPlayer3.Tx.Background_Up_1st[i].t2D描画(TJAPlayer3.app.Device, 0 - this.ct上背景スクロール用タイマー1st[i].n現在の値, (185 + i * 500) - (W * TJAPlayer3.Tx.Background_Up_1st[i].szテクスチャサイズ.Height) + ct上背景スクロール用タイマー2nd[i].n現在の値);
                    }
                    for (int l = 1; l < ForLoopL + 1; l++)
                    {
                        for (int W = 1; W < ForLoopW + 1; W++)
                        {
                            TJAPlayer3.Tx.Background_Up_1st[i].t2D描画(TJAPlayer3.app.Device, +(l * TJAPlayer3.Tx.Background_Up_1st[i].szテクスチャサイズ.Width) - this.ct上背景スクロール用タイマー1st[i].n現在の値, (185 + i * 500) - (W * TJAPlayer3.Tx.Background_Up_1st[i].szテクスチャサイズ.Height) + ct上背景スクロール用タイマー2nd[i].n現在の値);
                        }
                    }

                    for (int W = 1; W < ForLoopW + 1; W++)
                    {
                        TJAPlayer3.Tx.Background_Up_2nd[i].t2D描画(TJAPlayer3.app.Device, 0 - this.ct上背景スクロール用タイマー1st[i].n現在の値, (370 + i * 500) - (W * TJAPlayer3.Tx.Background_Up_2nd[i].szテクスチャサイズ.Height) - ct上背景スクロール用タイマー2nd[i].n現在の値);
                    }
                    for (int l = 1; l < ForLoopL + 1; l++)
                    {
                        for (int W = 1; W < ForLoopW + 1; W++)
                        {
                            TJAPlayer3.Tx.Background_Up_2nd[i].t2D描画(TJAPlayer3.app.Device, +(l * TJAPlayer3.Tx.Background_Up_2nd[i].szテクスチャサイズ.Width) - this.ct上背景スクロール用タイマー1st[i].n現在の値, (370 + i * 500) - (W * TJAPlayer3.Tx.Background_Up_2nd[i].szテクスチャサイズ.Height) - ct上背景スクロール用タイマー2nd[i].n現在の値);
                        }
                    }

                    #endregion

                    #region [ 上背景-Front ]

                    float thirdy = 0;
                    float thirdx = 0;

                    if(this.ct上背景スクロール用タイマー3rd[i].n現在の値 <= 54)
                    {
                        thirdx = this.ct上背景スクロール用タイマー3rd[i].n現在の値 * 4.629f;  
                        thirdy = (float)Math.Sin((float)(this.ct上背景スクロール用タイマー3rd[i].n現在の値 * 3.333f) * (Math.PI / 180)) * 50.0f;
                    }
                    else
                    {
                        thirdx = 250 + (ct上背景スクロール用タイマー3rd[i].n現在の値 - 54) * 1.244f;
                        if(this.ct上背景スクロール用タイマー3rd[i].n現在の値 <= 99)
                        {
                            thirdy = -(float)Math.Sin((float)((this.ct上背景スクロール用タイマー3rd[i].n現在の値 - 54) * 4) * (Math.PI / 180)) * 20.0f;
                        }
                        else if (this.ct上背景スクロール用タイマー3rd[i].n現在の値 <= 130)
                        {
                            thirdy = -(float)Math.Sin((float)((this.ct上背景スクロール用タイマー3rd[i].n現在の値 - 100) * 3) * (Math.PI / 180)) * 12.0f;
                        }
                    }

                    TJAPlayer3.Tx.Background_Up_3rd[i].t2D描画(TJAPlayer3.app.Device, 0 - thirdx, 0 + i * 550 - thirdy);

                    for (int l = 1; l < ForLoopF + 1; l++)
                    {
                        TJAPlayer3.Tx.Background_Up_3rd[i].t2D描画(TJAPlayer3.app.Device, +(l * TJAPlayer3.Tx.Background_Up_3rd[i].szテクスチャサイズ.Width) - thirdx, 0 + i * 550 - thirdy);
                    }

                    #endregion
                }

                if (this.ct上背景スクロール用タイマー1st[i] != null)
                {
                    if (TJAPlayer3.stage演奏ドラム画面.bIsAlreadyCleared[i])
                    {
                        TJAPlayer3.Tx.Background_Up_1st[2].Opacity = ((this.ct上背景クリアインタイマー[i].n現在の値 * 0xff) / 100);
                        TJAPlayer3.Tx.Background_Up_2nd[2].Opacity = ((this.ct上背景クリアインタイマー[i].n現在の値 * 0xff) / 100);
                        TJAPlayer3.Tx.Background_Up_3rd[2].Opacity = ((this.ct上背景クリアインタイマー[i].n現在の値 * 0xff) / 100);
                    }
                    else
                    {
                        TJAPlayer3.Tx.Background_Up_1st[2].Opacity = 0;
                        TJAPlayer3.Tx.Background_Up_2nd[2].Opacity = 0;
                        TJAPlayer3.Tx.Background_Up_3rd[2].Opacity = 0;
                    }

                    double TexSizeL = 1280 / TJAPlayer3.Tx.Background_Up_1st[2].szテクスチャサイズ.Width;
                    double TexSizeW = 308 / TJAPlayer3.Tx.Background_Up_2nd[2].szテクスチャサイズ.Height;
                    double TexSizeF = 1280 / TJAPlayer3.Tx.Background_Up_3rd[2].szテクスチャサイズ.Width;
                    // 1280をテクスチャサイズで割ったものを切り上げて、プラス+1足す。
                    int ForLoopL = (int)Math.Ceiling(TexSizeL) + 1;
                    int ForLoopW = (int)Math.Ceiling(TexSizeW) + 1;
                    int ForLoopF = (int)Math.Ceiling(TexSizeF) + 1;

                    #region [ 上背景-Back ]

                    for (int W = 1; W < ForLoopW + 1; W++)
                    {
                        TJAPlayer3.Tx.Background_Up_1st[2].t2D描画(TJAPlayer3.app.Device, 0 - this.ct上背景スクロール用タイマー1st[i].n現在の値, (185 + i * 640) - (W * TJAPlayer3.Tx.Background_Up_1st[2].szテクスチャサイズ.Height) + ct上背景スクロール用タイマー2nd[i].n現在の値);
                    }
                    for (int l = 1; l < ForLoopL + 1; l++)
                    {
                        for (int W = 1; W < ForLoopW + 1; W++)
                        {
                            TJAPlayer3.Tx.Background_Up_1st[2].t2D描画(TJAPlayer3.app.Device, +(l * TJAPlayer3.Tx.Background_Up_1st[2].szテクスチャサイズ.Width) - this.ct上背景スクロール用タイマー1st[i].n現在の値, (185 + i * 640) - (W * TJAPlayer3.Tx.Background_Up_1st[2].szテクスチャサイズ.Height) + ct上背景スクロール用タイマー2nd[i].n現在の値);
                        }
                    }

                    for (int W = 1; W < ForLoopW + 1; W++)
                    {
                        TJAPlayer3.Tx.Background_Up_2nd[2].t2D描画(TJAPlayer3.app.Device, 0 - this.ct上背景スクロール用タイマー1st[i].n現在の値, (370 + i * 640) - (W * TJAPlayer3.Tx.Background_Up_2nd[2].szテクスチャサイズ.Height) - ct上背景スクロール用タイマー2nd[i].n現在の値);
                    }
                    for (int l = 1; l < ForLoopL + 1; l++)
                    {
                        for (int W = 1; W < ForLoopW + 1; W++)
                        {
                            TJAPlayer3.Tx.Background_Up_2nd[2].t2D描画(TJAPlayer3.app.Device, +(l * TJAPlayer3.Tx.Background_Up_2nd[2].szテクスチャサイズ.Width) - this.ct上背景スクロール用タイマー1st[i].n現在の値, (370 + i * 640) - (W * TJAPlayer3.Tx.Background_Up_2nd[2].szテクスチャサイズ.Height) - ct上背景スクロール用タイマー2nd[i].n現在の値);
                        }
                    }

                    #endregion

                    #region [ 上背景-Front ]

                    float thirdy = 0;
                    float thirdx = 0;

                    if (this.ct上背景スクロール用タイマー3rd[i].n現在の値 <= 54)
                    {
                        thirdx = this.ct上背景スクロール用タイマー3rd[i].n現在の値 * 4.629f;
                        thirdy = (float)Math.Sin((float)(this.ct上背景スクロール用タイマー3rd[i].n現在の値 * 3.333f) * (Math.PI / 180)) * 50.0f;
                    }
                    else
                    {
                        thirdx = 250 + (ct上背景スクロール用タイマー3rd[i].n現在の値 - 54) * 1.244f;
                        if (this.ct上背景スクロール用タイマー3rd[i].n現在の値 <= 99)
                        {
                            thirdy = -(float)Math.Sin((float)((this.ct上背景スクロール用タイマー3rd[i].n現在の値 - 54) * 4) * (Math.PI / 180)) * 20.0f;
                        }
                        else if (this.ct上背景スクロール用タイマー3rd[i].n現在の値 <= 130)
                        {
                            thirdy = -(float)Math.Sin((float)((this.ct上背景スクロール用タイマー3rd[i].n現在の値 - 100) * 3) * (Math.PI / 180)) * 12.0f;
                        }
                    }

                    TJAPlayer3.Tx.Background_Up_3rd[2].t2D描画(TJAPlayer3.app.Device, 0 - thirdx, 0 + i * 640 - thirdy);

                    for (int l = 1; l < ForLoopF + 1; l++)
                    {
                        TJAPlayer3.Tx.Background_Up_3rd[2].t2D描画(TJAPlayer3.app.Device, +(l * TJAPlayer3.Tx.Background_Up_3rd[2].szテクスチャサイズ.Width) - thirdx, 0 + i * 640 - thirdy);
                    }

                    #endregion
                }
            }
            #endregion
            #region 1P-下背景
            if (!TJAPlayer3.stage演奏ドラム画面.bDoublePlay)
            {
                if (TJAPlayer3.Tx.Background_Down != null)
                {
                    TJAPlayer3.Tx.Background_Down.t2D描画(TJAPlayer3.app.Device, 0, 360);
                }
                if (TJAPlayer3.stage演奏ドラム画面.bIsAlreadyCleared[0])
                {
                    if (TJAPlayer3.Tx.Background_Down_Clear != null && TJAPlayer3.Tx.Background_Down_Scroll != null && ct下背景スクロール用タイマー1 != null)
                    {
                        TJAPlayer3.Tx.Background_Down_Clear.Opacity = ((this.ct上背景FIFOタイマー.n現在の値 * 0xff) / 100);
                        TJAPlayer3.Tx.Background_Down_Scroll.Opacity = ((this.ct上背景FIFOタイマー.n現在の値 * 0xff) / 100);
                        TJAPlayer3.Tx.Background_Down_Clear.t2D描画(TJAPlayer3.app.Device, 0, 360);

                        //int nループ幅 = 1257;
                        //CDTXMania.Tx.Background_Down_Scroll.t2D描画( CDTXMania.app.Device, 0 - this.ct下背景スクロール用タイマー1.n現在の値, 360 );
                        //CDTXMania.Tx.Background_Down_Scroll.t2D描画(CDTXMania.app.Device, (1 * nループ幅) - this.ct下背景スクロール用タイマー1.n現在の値, 360);
                        double TexSize = 1280 / TJAPlayer3.Tx.Background_Down_Scroll.szテクスチャサイズ.Width;
                        // 1280をテクスチャサイズで割ったものを切り上げて、プラス+1足す。
                        int ForLoop = (int)Math.Ceiling(TexSize) + 1;

                        //int nループ幅 = 328;
                        TJAPlayer3.Tx.Background_Down_Scroll.t2D描画(TJAPlayer3.app.Device, 0 - this.ct下背景スクロール用タイマー1.n現在の値, 360);
                        for (int l = 1; l < ForLoop + 1; l++)
                        {
                            TJAPlayer3.Tx.Background_Down_Scroll.t2D描画(TJAPlayer3.app.Device, +(l * TJAPlayer3.Tx.Background_Down_Scroll.szテクスチャサイズ.Width) - this.ct下背景スクロール用タイマー1.n現在の値, 360);
                        }

                    }
                }
            }
            #endregion
            return base.On進行描画();
        }

        #region[ private ]
        //-----------------
        private CCounter[] ct上背景スクロール用タイマー1st; //上背景のX方向スクロール用
        private CCounter[] ct上背景スクロール用タイマー2nd; //上背景のY方向スクロール用
        private CCounter[] ct上背景スクロール用タイマー3rd; //上背景のY方向スクロール用
        private CCounter ct下背景スクロール用タイマー1; //下背景パーツ1のX方向スクロール用
        private CCounter ct上背景FIFOタイマー;
        private CCounter[] ct上背景クリアインタイマー;
        //private CTexture tx上背景メイン;
        //private CTexture tx上背景クリアメイン;
        //private CTexture tx下背景メイン;
        //private CTexture tx下背景クリアメイン;
        //private CTexture tx下背景クリアサブ1;
        private EFIFOモード eFadeMode;
        //-----------------
        #endregion
    }
}