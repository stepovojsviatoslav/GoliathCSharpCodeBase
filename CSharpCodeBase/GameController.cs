using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using MainGame.core;
using UnityEngine.EventSystems;
namespace MainGame
{
    public class GameController:MonoBehaviour
    {

        public EntityFactory entityFactory;
        public float gameTime = 0;
        public static Player player;

        void Start()
        {
            //function GameController:Start()
            //self._coroutine = coroutine.create(function () self:Initialize() end)
            StartCoroutine("Initialize");
            //end
        }
        void Awake()
        {

            /*function GameController:Awake(gameObject)
                self.gameObject = gameObject
               end*/
        }

        IEnumerator Initialize()
        {
            //function GameController:Initialize()
            
            if(GameFacade.console!=null)
            {
                //Console:Show()
                GameFacade.console.Show();
                //Console:Message("Initialize services...");coroutine.yield()
                GameFacade.console.Message("Initialize services...");
                yield return null;
                //-- Initialize services
                entityFactory = new EntityFactory();
                //this.eventSystem = EventSystem();
                  this.database = Database();
                  this.worldGenerator = WorldGenerator();
                  this.pooler = Transform.AddComponent(UnityEngine.GameObject('Pooler'), 'Pooler');
                  this.luaPooler = LuaPooler();
                  this.gameTime = 0;
                  //this.cli = luanet.GameFacade.cli;
                  // override printing to console --?
                  //local Log = luanet.UnityEngine.Debug.Log
                  /*_G['print'] = function (data)
                    self.cli:Print(data)
                    Debug.Log(data)
                  end*/
                  GameFacade.console.Message("Create physic materials...");
                  yield return null;
                    
                  //luanet.PhysicMaterialManager.CreateMaterial("terrain", 1, luanet.UnityEngine.PhysicMaterialCombine.Maximum)
                  PhysicMaterialManager.CreateMaterial("terrain", 1, PhysicMaterialCombine.Maximum);
                  //luanet.PhysicMaterialManager.CreateMaterial("nofriction", 0, luanet.UnityEngine.PhysicMaterialCombine.Minimum)
                  PhysicMaterialManager.CreateMaterial("nofriction", 0, UnityEngine.PhysicMaterialCombine.Minimum);
                    this.defaultPhysicMaterial = 'nofriction';
                  // Load configs
                  GameFacade.console.Message("Load configuration files:");yield return null;
                  string[] iniFiles = {
                      "data/config/shaders.ini",
                      "data/config/bioms.ini",
                      "data/config/groups.ini",
                      "data/config/scatter.ini",
                      "data/config/prefabs.ini",
                      "data/config/enemies.ini",
                      "data/config/heroes.ini",
                      "data/config/components.ini",
                      "data/config/daytime.ini",
                      "data/config/weapons.ini",
                      "data/config/nature.ini",
                      "data/config/items.ini",
                      "data/config/worlds.ini",
                      "data/config/damagecolors.ini",
                      "data/config/drop.ini",
                      "data/config/spells.ini",
                      "data/config/constructions.ini",
                      "data/config/gadget.ini",
                      "data/config/bafs.ini"
                    };
                    foreach(string v in pairs(iniFiles))
                    {
                      this.database.Load(v);
                    }

                  //Load locales
                  GameFacade.console.Message("Setup locale...");yield return null;  
                  this.database:Load("data/config/locale.dat")  
                  this.locale = this.database:Get("locale", "ru")
  
                  GameFacade.console.Message("Setup shaders...");yield return null;
                  ShaderUtils.SetGlobalTexture("_CloudMap", this.database:Get("shaders", "global/CloudMap"))
                  ShaderUtils.SetGlobalFloat("_CloudScale", this.database:Get("shaders", "global/CloudScale"))
                  ShaderUtils.SetGlobalFloat("_CloudSpeed", this.database:Get("shaders", "global/CloudSpeed"))
  
                  GameFacade.console.Message("Init user interface...");yield return null;    
                  this:TryCall(this.InitUI, this)
                  GameFacade.console.Message("Initialize prefabs...");yield return null;
                  this:InitPrefabs()
                  GameFacade.console.Message("Create pools...");yield return null;
                  for k,v in pairs(this.pools) do 
                    this:InitPool(v) 
                  end
                  GameFacade.console.Message("Create world stream");yield return null;
                  this.worldController = Transform.AddComponent(UnityEngine.GameObject("WorldController"), "StreamController")
                  GameFacade.console.Message("Generate new world");yield return null;
                  local worldGenerationCoroutine = this.worldGenerator:GetCreateWorldCoroutine()
                  while coroutine.status(worldGenerationCoroutine) ~= 'dead' do
                    coroutine.resume(worldGenerationCoroutine)
                    yield return null;
                  end
                  GameFacade.console.Message("Create scene light and other services...");yield return null;
                  this:TryCall(function ()
                    this.daytime = DayTime()
                    this.timeService = TimeService()
                    this.sceneLight = SceneLight(luanet.UnityEngine.GameObject.Find("Directional light"))
                    this.dropManager = DropManager()
                    this.spellSystem = SpellSystem()
                    this.inventory = Inventory()
                    this.inventory:AddItem('item', 2)
                    this.inventory:AddItem('hero_axe', 3)
                    this.inventory:AddItem('grenade1', 4)
                    this.inventory:AddItem('grenade2', 4)
                    this.inventory:AddItem('grenade3', 4)
                    this.inventory:AddItem('grenade4', 4)
                    this.inventory:AddItem('grenade5', 4)
                    this.inventory:AddItem('grenade6', 4)
                    this.inventory:AddItem('grenade6', 4)
                    this.inventory:AddItem('small_medicine_chest', 4)
                    this.inventory:AddItem('big_medicine_chest', 4)
                    this.inventory:AddItem('normal_medicine_chest', 4)
                    this.inventory:AddItem('small_repair_kit', 4)
                    this.inventory:AddItem('big_repair_kit', 4)
                    this.inventory:AddItem('normal_repair_kit', 4)
                  end)
                  GameFacade.console.Message("Create character and sync world...");yield return null;
                  this:TryCall(this.CreateCharacter, this)  
                  this.worldController:ProcessTasksSync()
                  GameFacade.console.Message("Place character into the world");yield return null;
                  local character = this.player.playerController:GetCurrentSlot()
                  local pos = character:GetPosition()
                  pos.y = RaycastUtils.GetHeight(pos)
                  character:SetPosition(pos)
                  this.player:StoreHomePosition(pos) 
                  //self:TryCall(self.RunTest, self, "BenchEntity")
                  this:TryCall(this.RunTest, this, "ChestEntity")
 
                  this.isStarted = true
                  GameFacade.console.Message("Complete!");yield return null;
                  Console:Hide()
                  //self:TestBuilding()
                  GameController.inventory:AddItem('hero_shoot', 1)  
                end
            yield return null;
            }
        }
        /*
         local GameController = Class({init = function (self)
  self.pools = {}
  self.ui = {}
  self.gameLoop = GameLoop(gameObject)
  self.objectMap = ObjectMap()  
  self.inputService = InputService()
  self.dialogueSystem = DialogueSystem()
end})







function GameController:TryCall(func, ...)
  local status, err = pcall(func, ...)
  if not status then print(err) end
  return status
end

-- Proxy methods
function GameController:Update()
  self.deltaTime = Time.deltaTime
  
  if self._coroutine ~= nil then
    if coroutine.status(self._coroutine) ~= 'dead' then
      coroutine.resume(self._coroutine)
    else
      self._coroutine = nil
    end
  else
    if Input.GetKeyDownUnlocked(KeyCode.BackQuote) then
      if Input.enabled then
        self.cli:Show()
        Input.enabled = false
      else
        self.cli:Hide()
        Input.enabled = true
      end
    end
    self.gameTime = self.gameTime + self.deltaTime
    self.daytime:Update()
    self.gameLoop:Update()
    self.timeService:Update()
    self.dialogueSystem:Update()
  end
  self.objectMap:Reindex()
end

function GameController:FixedUpdate()
  self.deltaTime = Time.deltaTime
  self.gameLoop:FixedUpdate()
end

function GameController:LateUpdate()
  self.deltaTime = Time.deltaTime
  self.gameLoop:LateUpdate()
end

-- Load and initialize ui
function GameController:InitUI()
  self.ui.mainInterface = MainInterface(luanet.GameFacade.mainInterface.gameObject)
  self:AddEntity(self.ui.mainInterface)
  self.ui.character = luanet.GameFacade.characterPanel
  --self.ui.character = luanet.GameFacade.uiCharacter
  ---self.ui.slotsController = UISlotsController(luanet.UnityEngine.GameObject.Find("UISlotsController"))
  --self:AddEntity(self.ui.slotsController)  
  self:AddEntity(UIBenchController(luanet.UnityEngine.GameObject.Find("UIBenchController")))  
  self:AddEntity(UIChestController(luanet.UnityEngine.GameObject.Find("UIChestController")))  
  
  self.gadgetController = UIGadgetController()
  self:AddEntity(self.gadgetController)
  self.weaponController = UIWeaponController(luanet.UnityEngine.GameObject.Find("UIWeaponPanel"))
  self:AddEntity(self.weaponController)
 
  --self.ui.spellPanel = luanet.GameFacade.uiSpellPanel
  --self.ui.spellPanel:Reset()
end

function GameController:CreateEntityFromPool(entityType, position)
  local classPath = self.database.data.prefabs[entityType].require
  local class = require(classPath)
  local object = self.pooler:Fetch(entityType)  
  local entity = class(object)
  entity:SetPosition(position)
  entity._type = entityType
  return entity
end

-- Load and save object from storage
function GameController:Load(storage, chunk)
  local entity = self.entityFactory:LoadFromStorage(storage, chunk)
  return entity.gameObject
end

function GameController:Save(obj)
  storage = luanet.Whalebox.WorldStream.Grid.SpaceStorage()
  local entity = luanet.LuaUtils.GetEntity(obj.transform)
  if entity == nil then
    print("Entity to save is nil: " .. obj.name)
  else
    entity:Save(storage)
    storage:SetString("type", entity._type)
  end
  
  return storage  
end

function GameController:InitPool(data)
  local obj = BundleUtils.CreateFromBundle(data.bundle)
  obj.name = data.name
  for k, v in pairs(data.components) do
    Transform.AddComponent(obj, v)
  end
  if data.visibility_radius ~= nil then
    Transform.GetMapper(obj):EnableFrustumSphere(data.visibility_radius)
  end
  if data.class and data.class.PrepareObject then
    data.class.PrepareObject(obj)
  end
  luanet.PhysicMaterialManager.SetupMaterial(obj, data.pmaterial or self.defaultPhysicMaterial)
  self.pooler:CreatePool(obj, data.capacity)  
end

function GameController:RegisterPool(data)
  if self.isStarted then
    self:InitPool(data)
  else
    self.pools[#self.pools + 1] = data
  end
end

function GameController:CreateCharacter() 
  self.camera = CameraEntity(UnityEngine.Camera.main.gameObject)
  CameraUtils.SetFOV(30)  
  self:AddEntity(self.camera)
  
  local gameObject = UnityEngine.GameObject("Player")
  gameObject.tag = 'Player'
  Transform.AddComponent(gameObject, "LuaMapper")
  self.player = Player(gameObject)  
  self.player:SetPosition(Vector3(1000, 50, 1000))
  self:AddEntity(self.player)
  
  local characterEntity = self.player.playerController:LoadSlot("Hero")  
  characterEntity:SetPosition(Vector3(1000, 50, 1000))  
  self.player.playerController:AddSlot(characterEntity)
  self.player.playerController:SelectSlot(1)
  local characterEntity1 = self.player.playerController:LoadSlot("IronMech_1")
  self.player.playerController:AddSlot(characterEntity1)  
  
  self.camera:SetTargetEntity(self.player)
  self.worldController:AddTargetTransform(self.player.transform)
end

function GameController:TestBuilding()
  local BuilderEntity = require ('entities.builderentity.builderentity')
  self.builder = BuilderEntity()
  self:AddEntity(self.builder)
end

function GameController:RunTest(NameOfTestEntity, position)
  if NameOfTestEntity then 
    local TestEntity = require (self.database:Get("prefabs", NameOfTestEntity .. '/require'))
    local x, y, z
    if position then
      x = position.x
      y = position.y
      z = position.z
    else
      local positionOfHero = self.player.homePosition
      x = positionOfHero.x + math.random() * 5 + 2
      y = positionOfHero.y
      z = positionOfHero.z + math.random() * 5 + 2
    end        
    local bundleName = self.database:Get("prefabs", NameOfTestEntity .. '/bundle')    
    local gameObject = BundleUtils.CreateFromBundle(bundleName)
    gameObject.name = NameOfTestEntity
    for k, v in pairs(self.database.data["prefabs"][NameOfTestEntity]["component"]) do
      Transform.AddComponent(gameObject, v)
    end    
    self.testInstance = TestEntity(gameObject)       
    self.testInstance:SetPosition(Vector3(x, y, z))
    local rotation = self.testInstance:GetRotation()
    rotation.y = math.random(0, 360)
    self.testInstance:SetRotation(rotation)    
    local scale = self.database:Get("prefabs", NameOfTestEntity .. '/demo_scale')
    if scale then
      self.testInstance:SetScale(Vector3(scale, scale, scale))
    end
    self.testInstance._type = NameOfTestEntity        
    self:AddEntity(self.testInstance)
  end
end

function GameController:InitPrefabs()
  for section, values in pairs(self.database.data.prefabs) do
    Console:Message("Initialize " .. section)

    local class
    if values.require then
      class = require(values.require)
      class._type = section
    end
    
    self:RegisterPool({
        class=class,
        bundle=values.bundle,
        components=values.component,
        name=section,
        capacity=values.pool_capacity,
        pmaterial=values.pmaterial,
        visibility_radius=values.visibility_radius,
        complex_visibility=values.complex_visibility})
  end
end

function GameController:GetFromDatabase(tbl, path)
  return self.database:Get(tbl, path)
end

function GameController:AddEntity(entity)
  self.gameLoop:Add(entity)
  if entity.transform ~= nil then
    self.objectMap:AddObject(entity)
    entity:ResetPositionCache()
    self.objectMap:CheckIndex(entity)
  end
end

function GameController:AddToWorld(entity)
  entity._inWorld = true
  self.worldController:RegisterTransform(entity.transform)
  if not entity.static then
    self.worldController:CheckForMigrations(entity.gameObject)
  end
end

function GameController:RemoveEntity(entity)
  self.gameLoop:Remove(entity)
  if entity.transform then
    self.objectMap:RemoveObject(entity)
  end
end

function GameController:RemoveFromWorld(entity)
  if not entity.static then
    self.worldController:UncheckForMigrations(entity.gameObject)
  end
  self.worldController:RemoveFromChunk(entity.gameObject)
end 
         */
    }
}
