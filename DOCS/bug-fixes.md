# MonsterAR Demo 修正表追蹤

## 📋 0714 Demo 修正表分析

### 如何使用此文件
1. 請將 PDF 修正表中的項目手動轉錄到下方表格
2. 標記哪些項目需要程式修正
3. 設定優先級和狀態

### 修正項目分類模板

#### 🔧 程式修正項目
| 編號 | 問題描述 | 影響範圍 | 優先級 | 狀態 | 預估時間 | 相關檔案 |
|------|----------|----------|--------|------|----------|----------|
| P001 | 降低惡魔的透明度 | 3D模型顯示 | 中 | 待處理 | 30分鐘 | SummonerDetected.cs, Material設定 |
| P002 | 刪除選擇卡片的音效 | 音效系統 | 低 | 待處理 | 15分鐘 | AudioManager.cs, ChoseTargetDetected.cs |
| P003 | 目前的介面會擋到惡魔的視線，希望可以縮小按鍵與畫面 | UI佈局 | 高 | 待處理 | 1小時 | UIManager.cs, UI Canvas設定 |
| P004 | 增加道具頁的按鍵音效 | 音效系統 | 低 | 待處理 | 20分鐘 | AudioManager.cs, 道具頁Script |
| P005 | 主換原本的橘色按鍵 | UI視覺 | 低 | 待處理 | 10分鐘 | Button Prefab, UI素材 |
| P006 | 更換「快速辨識」按鍵 | UI視覺 | 低 | 已完成 | 10分鐘 | FastUnlockButton.cs, UI素材 |
| P007 | 增加「現身」按鍵 | 功能新增 | 中 | 待處理 | 45分鐘 | SummonerDetected.cs, UI系統 |
| P008 | 重新進入後，選擇卡片組合，先按「現身」按鍵 | 遊戲流程 | 高 | 待處理 | 1小時 | DemonsDetectedManager.cs, 狀態管理 |
| P009 | 「封印」按鍵是錯誤的，呈現不能按的功能 | 按鍵功能 | 高 | 待處理 | 30分鐘 | UIManager.cs, Button狀態管理 |
| P010 | 增加選擇卡牌的音效 | 音效系統 | 低 | 待處理 | 15分鐘 | AudioManager.cs, 卡牌選擇Script |
| P011 | 目前數字的辨識度太低，所以要更換圖片 | UI視覺 | 中 | 待處理 | 20分鐘 | UI素材替換 |
| P012 | 「封印」按鍵裝饰(可以使用) | 按鍵功能 | 高 | 待處理 | 45分鐘 | UIManager.cs, 按鍵狀態邏輯 |
| P013 | 增加選擇正確的音效 | 音效系統 | 低 | 待處理 | 15分鐘 | AudioManager.cs, 成功判定邏輯 |
| P014 | 讓惡魔顯現出來 | AR顯示 | 高 | 待處理 | 30分鐘 | SummonerDetected.cs, 3D模型控制 |
| P015 | 修改可以增加放大縮小效果 | 3D互動 | 中 | 待處理 | 1小時 | SummonerDetected.cs, 手勢控制 |
| P016 | 按下「封印」按鍵導向成功頁面 | 場景切換 | 高 | 待處理 | 30分鐘 | SceneTransition.cs, 流程控制 |

#### 🎨 美術/UI 修正項目
| 編號 | 問題描述 | 影響範圍 | 優先級 | 狀態 | 預估時間 | 相關檔案 |
|------|----------|----------|--------|------|----------|----------|
| U001 | [範例] 按鈕顏色不正確 | UI 顯示 | 低 | 待處理 | 15分鐘 | UI Prefabs |
| U002 | [範例] 文字顯示問題 | UI 文字 | 中 | 待處理 | 20分鐘 | TextMeshPro |

#### 📱 平台相容性問題
| 編號 | 問題描述 | 影響範圍 | 優先級 | 狀態 | 預估時間 | 相關檔案 |
|------|----------|----------|--------|------|----------|----------|
| C001 | [範例] Android 版本相容性 | 平台相容 | 高 | 待處理 | 1小時 | Build Settings |

#### 🎮 遊戲體驗問題
| 編號 | 問題描述 | 影響範圍 | 優先級 | 狀態 | 預估時間 | 相關檔案 |
|------|----------|----------|--------|------|----------|----------|
| G001 | [範例] 遊戲流程不順暢 | 用戶體驗 | 中 | 待處理 | 45分鐘 | UIManager.cs |

---

## 🔍 常見程式問題類型

### AR 相關問題
- **識別不穩定**：檢查 `SummonerDetected.cs` 中的追蹤邏輯
- **模型顯示異常**：檢查 3D 模型載入和材質設定
- **攝影機權限**：檢查 Android/iOS 權限設定

### 遊戲邏輯問題
- **配對邏輯錯誤**：檢查 `DemonsDetectedManager.cs:141-188`
- **生命值系統**：檢查 HP 減少和遊戲結束邏輯
- **狀態同步**：檢查 PlayerPrefs 讀寫

### UI/UX 問題
- **按鈕響應**：檢查 Button 元件和 EventSystem
- **場景切換**：檢查 `SceneTransition.cs`
- **文字顯示**：檢查 TextMeshPro 設定

### 音效問題
- **音效不播放**：檢查 `AudioManager.cs` 和音檔路徑
- **音量控制**：檢查音量設定和 AudioMixer
- **背景音樂**：檢查 BGM 迴圈和淡入淡出

---

## 🚀 緊急修復優先級

### 🔴 高優先級（必須修復）
- 應用程式崩潰問題
- AR 無法識別
- 核心遊戲邏輯錯誤
- 無法正常進行遊戲

### 🟡 中優先級（重要但不緊急）
- UI 顯示問題
- 音效播放問題
- 用戶體驗優化
- 性能問題

### 🟢 低優先級（如果時間允許）
- 視覺效果優化
- 文字修正
- 小幅 UI 調整
- 非核心功能增強

---

## 📝 修正流程

### 1. 問題分析
- 重現問題
- 定位問題原因
- 評估影響範圍

### 2. 修復方案
- 選擇最安全的修復方式
- 避免大幅重構
- 優先使用現有程式碼

### 3. 測試驗證
- 功能測試
- 回歸測試
- 不同設備測試

### 4. 記錄文檔
- 更新工作日誌
- 記錄修復方案
- 更新已知問題列表

---

## 🔧 快速修復程式碼模板

### AR 識別穩定性修復
```csharp
// 在 SummonerDetected.cs 中添加防護
private void Update() {
    if (trackingStatus == TrackingStatus.TRACKING) {
        // 確保模型穩定顯示
        if (!SummonerObject.activeInHierarchy) {
            SummonerObject.SetActive(true);
        }
    }
}
```

### 卡片配對邏輯防護
```csharp
// 在 DemonsDetectedManager.cs 中添加驗證
private bool ValidateCardSelection() {
    if (R < 1 || R > 8 || O < 1 || O > 8 || A < 1 || A > 8) {
        Debug.LogError("Invalid card selection");
        return false;
    }
    return true;
}
```

### 音效播放防護
```csharp
// 在 AudioManager.cs 中添加 null 檢查
public void PlayAudio(AudioClip clip) {
    if (clip != null && audioSource != null) {
        audioSource.clip = clip;
        audioSource.Play();
    } else {
        Debug.LogWarning("AudioClip or AudioSource is null");
    }
}
```

---

*請將 PDF 修正表中的具體項目填入上方表格，我會協助你處理程式相關的修正工作。*