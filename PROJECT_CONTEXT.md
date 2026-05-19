# SoloProjectM Project Context

Branch: Develop
Purpose: Reference file for AI assistants before changing scenes, UI, scripts, or data flow.

## 1. Core folders to inspect first

- Assets/Scripts
- Assets/Scenes
- Assets/Resources

For SPUM related work, also inspect these folders when needed.

- Assets/SPUM
- Assets/WebAsset/SPUM

Do not assume a new hierarchy. Use existing scene objects first.

## 2. Scene files

- Assets/Scenes/TitleScene.unity
- Assets/Scenes/RoomScene.unity
- Assets/Scenes/InGameScene.unity

## 3. TitleScene role

TitleScene is the login and account entry scene.

Main responsibilities:

- Initialize Firebase
- Login
- Register account
- Load PlayerProfileData from Firebase Realtime Database
- Store profile in GameManager
- Move to RoomScene

Related scripts:

- FirebaseAuthMgr
- RegisterUI
- GameManager

Important note:

FirebaseAuth.CurrentUser can remain from a previous session. If TryLoadCurrentUserProfile is called after Firebase initialization, the profile can be loaded before pressing the login button. This can look like login happened automatically.

## 4. RoomScene role

RoomScene is the lobby and out-game scene.

Main responsibilities:

- Shop
- Equipment view
- Inventory
- Character profile and stat view
- Adventure or scenario select
- Stat upgrade
- Cash or diamond related UI

Current RoomScene hierarchy reference:

```text
RoomScene
- Main Camera
- EventSystem
- MainCanvas
  - Scroll View
    - Viewport
      - Content
        - Shop
        - Equip
          - Use
            - UsingView
              - Unit
                - EquipCharacterRawImage
              - Slots
              - State
        - Inven
          - InvenPanel
            - Scroll View
              - Viewport
                - InvenContent
              - Scrollbar Vertical
                - Sliding Area
                  - Handle
            - Button
        - Adventure
          - AdventureCharacterRawImage
        - StateUp
        - Cash
    - Scrollbar Horizontal
  - Image
  - Bottom Menu
  - Top Menu
  - Settings
  - option
- ParallaxBackgroundCtrl
- StageSelectionCtrl
- StageManager
- RoomProfileUI
- RoomFlowController
- RoomSceneUI
- DontDestroyOnLoad
```

## 5. RoomScene UI attachment rules

Use these existing objects first.

```text
RoomProfileUI script:
- Attach to existing RoomProfileUI object, or to Equip > Use > UsingView if the scene is reorganized later.
- It reads GameManager.Instance.PlayerProfile.
- It updates nickname, currencies, HP, attack, and defense UI.

Equip character display:
- UI location: Equip > Use > UsingView > Unit > EquipCharacterRawImage
- Component: RawImage
- RawImage texture: RT_EquipCharacter

Adventure character display:
- UI location: Adventure > AdventureCharacterRawImage
- Component: RawImage
- RawImage texture: RT_AdventureCharacter
```

Do not put the SPUM character prefab directly under Unit or Adventure UI. These are UI objects.

## 6. RenderTexture preview structure

SPUM characters are SpriteRenderer based world objects. They should not be directly placed under Canvas UI. Use a preview world and RenderTexture.

Recommended preview hierarchy outside MainCanvas:

```text
CharacterPreviewWorld
- EquipPreview
  - EquipPreviewCamera
  - EquipCharacterSpawnPoint
- AdventurePreview
  - AdventurePreviewCamera
  - AdventureCharacterSpawnPoint
```

Attach scripts:

```text
EquipPreview:
- CharacterPrefabPreview
- characterParent = EquipCharacterSpawnPoint
- defaultPrefabPath = SPUM_20250417115258389
- initialState = IDLE
- initialAnimationIndex = 0

AdventurePreview:
- CharacterPrefabPreview
- characterParent = AdventureCharacterSpawnPoint
- defaultPrefabPath = SPUM_20250417115258389
- initialState = MOVE
- initialAnimationIndex = 0
```

RenderTexture assets:

```text
RT_EquipCharacter
- Assigned to EquipPreviewCamera.targetTexture
- Assigned to EquipCharacterRawImage.texture

RT_AdventureCharacter
- Assigned to AdventurePreviewCamera.targetTexture
- Assigned to AdventureCharacterRawImage.texture
```

## 7. Resources path rule

Current SPUM prefab location:

```text
Assets/Resources/SPUM_20250417115258389.prefab
```

Correct Resources.Load path:

```text
SPUM_20250417115258389
```

Wrong paths:

```text
Assets/Resources/SPUM_20250417115258389.prefab
Units/SPUM_20250417115258389
```

The Units path is only valid if the prefab is moved to:

```text
Assets/Resources/Units/SPUM_20250417115258389.prefab
```

## 8. Login and profile data flow

```text
TitleScene
FirebaseAuthMgr
Firebase Authentication login success
Realtime Database /users/{uid} load
PlayerProfileData deserialize
GameManager.Instance.SetPlayerProfile(profileData)
RoomScene load
RoomProfileUI.Refresh
Read GameManager.Instance.PlayerProfile
Update TMP UI and character previews
```

## 9. Firebase Realtime Database profile shape

```text
users
- {uid}
  - uid
  - nickname
  - energy
  - diamonds
  - gold
  - stats
    - level
    - maxHp
    - attack
    - speed
    - attackCount
    - defense
  - appearance
    - characterPrefabId
    - portraitId
    - bodyId
    - eyeId
    - hairId
    - clothId
    - weaponId
    - shieldId
  - items
  - skills
```

Existing user data may not have appearance. NormalizeProfile should create a default appearance when it is missing.

## 10. Main script roles

### GameManager

Responsibilities:

- Singleton
- DontDestroyOnLoad
- Scene loading
- Store PlayerProfileData
- Clear PlayerProfileData on logout

Important methods:

- SetPlayerProfile(PlayerProfileData profile)
- ClearPlayerProfile()
- SceneLoad(SceneName sceneName)

### FirebaseAuthMgr

Responsibilities:

- Firebase initialization
- Login
- Register UI entry
- Load profile from Firebase
- Create default profile if missing
- Store profile through GameManager
- Logout

Logout should call both FirebaseAuth.SignOut and GameManager.ClearPlayerProfile.

### PlayerProfileData

Firebase stored account and character data.

Fields:

- uid
- nickname
- energy
- diamonds
- gold
- stats
- appearance
- items
- skills

### PlayerStatsData

Character stat data.

Fields:

- level
- maxHp
- attack
- speed
- attackCount
- defense

### PlayerAppearanceData

Character visual selection data.

Fields:

- characterPrefabId
- portraitId
- bodyId
- eyeId
- hairId
- clothId
- weaponId
- shieldId

Current first implementation mainly uses characterPrefabId. Body, eye, hair, cloth, weapon, and shield IDs are for future part based customization.

### RoomProfileUI

RoomScene profile UI controller.

Responsibilities:

- Read GameManager.Instance.PlayerProfile
- Update TMP nickname and currency UI
- Update HP, attack, defense UI
- Refresh EquipPreview and AdventurePreview character display

Text components should use TMP_Text, not UnityEngine.UI.Text.

### CharacterPrefabPreview

Preview character loader.

Responsibilities:

- Read PlayerAppearanceData or GameManager profile
- Load prefab by Resources.Load
- Instantiate under a preview spawn point
- Initialize SPUM_Prefabs animation lists
- Play initial PlayerState

This script belongs on preview world objects, not on RawImage objects.

### RoomSceneUI

Despite its name, this is not the RoomScene profile UI. It is closer to a run HUD UI.

Responsibilities:

- Display RunManager.RunState
- Day
- HP
- run gold
- attack
- encounter
- reward preview

Possible future rename:

- RunHudUI
- RunStatusUI

### Player

Runtime combat character controller.

Responsibilities:

- SPUM animation initialization
- Animation playback
- UnitStats access for combat

Player is not the Firebase profile data object.

### UnitStats

Runtime combat stats.

Used by combat systems and ICombatant.

## 11. SPUM usage policy

The goal is not to use SPUM only through its editor workflow.

Current policy:

- Use SPUM image and animation resources.
- Minimize direct dependency on SPUM_Manager editor flow.
- Use game-specific wrapper scripts where possible.
- First implementation uses one Resources prefab for preview.
- Later implementation can map PlayerAppearanceData part IDs to SPUM parts.

## 12. Do and do not

Do:

- Check current scene hierarchy before suggesting new objects.
- Use existing RoomScene UI structure.
- Keep account profile data in GameManager.PlayerProfile.
- Keep preview character instances separate from profile data.
- Use RenderTexture and RawImage for UI character previews.
- Use TMP_Text for text UI.

Do not:

- Put SPUM prefab directly under Canvas UI.
- Put CharacterPrefabPreview on RawImage objects.
- Use Assets/Resources or .prefab in Resources.Load path.
- Mix RunManager.RunState with Firebase PlayerProfileData.
- Create new root UI objects without checking the existing hierarchy.

## 13. Current intended flow

```text
Login
Load PlayerProfileData
Store in GameManager
RoomScene opens
RoomProfileUI reads profile
RoomProfileUI updates profile TMP values
RoomProfileUI refreshes EquipPreview and AdventurePreview
EquipPreview loads SPUM_20250417115258389 and plays IDLE
AdventurePreview loads SPUM_20250417115258389 and plays MOVE
Preview cameras render to RenderTextures
RawImages display RenderTextures in the sliding UI screens
```
