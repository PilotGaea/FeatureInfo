# FeatureInfo 範例專案檔
PilotGaea O’view Map Server FeatureInfo Plugin

[開發者文件](https://nlscsample.pilotgaea.com.tw/demo/ProgrammingGuide/src/04.ServerSidePlugin/04.2_PluginSample.html#featureinfo)

## Map Server Plugin 介紹：

在O’view Map Server下，共有以下四種類型的外掛可供自訂：

1. DoCommand 自訂操作供使用者呼叫，實作DoCmdBaseClass
2. Account 實作帳號登入功能，實作AccountBaseClass
3. FeatureInfo 提供查詢WMTS圖素屬性，實作FeatureInfoBaseClass
4. Snap鎖點功能，實作SnapBaseClass

以上幾種外掛，DoCommand可多個同時起作用，而Account、FeatureInfo、Snap則同一時間內只啟動一個。

Map Server在啟動或Plugin目錄被重新設定時，會重新開始搜尋指定目錄下的dll檔，根據其實作的BaseClass建立其實體且將其初始化。

在載入dll的過程中如果拋出了例外，當下讀取中的class將被捨棄；各個外掛都是獨立的，即使其中的class名稱重複也不影響讀取，但如果設定的指令重複，後面讀取的會蓋過前面讀取的；同一時間只啟動一個的外掛，後面讀的也是會蓋過前面的。

當DoCommand執行過程中如果拋出了例外，呼叫者會收到HTTP 500 Internal Server Error的回應，並且Map Server會將此例外會寫入log。

如果使用Web做為前端，請在client各個新增圖層處加上proxy引數，目的是為了在呼叫外掛相關功能時能夠讓cookie正確被設定。

>**注意事項：**
>
> 所有的外掛在編譯時都需注意輸出的dll檔是32位元或是64位元，跟安裝的Map Server必須是同樣位元組的版本，否則讀取外掛時會失敗。

## FeatureInfo 使用方法：

1. 使用MicroSoft Visual Studio 2017(或以上之版本)開啟專案檔。
2. 開始建置專案。(須注意Debug/Release與CPU版本。)
3. 完成後請到專案資料夾中的`\bin\Debug`(或是`\bin\Release`，依建置類型決定)目錄內將檔案複製到安裝目錄下的`plugins`目錄中。
4. 最後用client進行呼叫查看執行結果。
5. 呼叫範例：

```html
http://127.0.0.1:8080/wmts?Layer=範例向量圖層&Request=GetFeatureInfo&TileMatrixSet=EPSG:3857&i=127&j=127&TileRow=877&TileCol=1713&TileMatrix=11&InfoFormat=application/json
```

> **補充說明：**<br/>
> 其他詳細參數如下列介紹：
>
> + Layer：要讀取資訊的圖層名稱。
> + TileMatrixSet：圖層右鍵編輯→進階設定就能看到。
> + i、j：在圖磚中的坐標。
> + TileRow、TileCol：圖磚縱、橫軸索引。
> + TileMatrix：圖磚資料層集合。
> + InfoFormat：回傳的結構。
