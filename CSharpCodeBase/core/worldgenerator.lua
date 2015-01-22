local Class = require "utils.hump.class"
local ConfigComponent = require 'components.services.config'

local WorldGenerator = Class({init = function (self)
      self.generator = luanet.LandscapeGenerator()
      self.config = ConfigComponent('worlds','SimpleWorld')
end})

function WorldGenerator:GetCreateWorldCoroutine()
  return coroutine.create(function ()
      self:CreateWorld()
  end)
end

function WorldGenerator:CreateWorld()
  -- создаем заготовку для карты
  Console:Message("Initialize world layers...")
  self.generator:AddLayer(luanet.CreateMapLayer(self.config:Get('mapName'), self.config:Get('mapQuadSize'), self.config:Get('mapWorldSize')))
  
  -- строим диаграмму Вороного (на основе графа Вороного)
  self.generator:AddLayer(luanet.VoronoiLayer({
    map = self.config:Get('mapName'),
    min_sites_count = self.config:Get('minSitesCount'),
    max_sites_count = self.config:Get('maxSitesCount'),
    relax_iterations = self.config:Get('relaxIterations'),
    base_radius = self.config:Get('baseRadius'),
    debug = G_Debug,
  }))  
  
  -- составляем таблицы коэффициентов распределения для групп биомов, параметров групп, общего состава биомов
  local i = 0
  local groupDistribution = {}  
  local groupNames = self.config:Get('groupName')
  local groupValues = self.config:Get('groupValue')
  local groupParams = {}
  local bioms = {}  
  
  while (i < #groupNames and i < #groupValues) do 
    i = i + 1    
    table.insert(groupDistribution, {name = groupNames[i], value = groupValues[i]})            
    local currentGroup = GameController.database.data.groups[groupNames[i]]
    table.insert(groupParams, currentGroup)
    local j = 0
    while j < #currentGroup['bioms'] do
      j = j + 1
      if not bioms[currentGroup['bioms'][j]] then
        bioms[currentGroup['bioms'][j]] = GameController.database.data.bioms[currentGroup['bioms'][j]]
      end      
    end
  end
  
  -- распределяем ячеейки диаграммы Вороного между биомами
  self.generator:AddLayer(luanet.VoronoiMarkBiomsLayer({
    map = self.config:Get('mapName'),
    groups = groupDistribution,
    noise_coef = self.config:Get('noiseCoef'),
    noise_offset = self.config:Get('noiseOffset'),
    debug = G_Debug,
  }))
  
  -- усекаем размер графа Вороного до заданного
  self.generator:AddLayer(luanet.VoronoiSeparatorLayer({
    map = self.config:Get('mapName'),
    points_count = self.config:Get('truncPointsCount'),
    min_distance = self.config:Get('truncMinDistance'),
    depth = self.config:Get('truncDepth'),
    debug = G_Debug,
  }))

  -- составляем карту биомов
  self.generator:AddLayer(luanet.VoronoiRasterization())
  
  -- составляем карту маркеров (островов ячеек одного биома), карту соседей для граничных точек областей, карту высот
  self.generator:AddLayer(luanet.VoronoiHeightMap())
  
  -- составляем список областей связности (в смысле достижимости) и изменяем их высоты для того, чтобы попытаться построить односвязный граф для всех ячеек
  self.generator:AddLayer(luanet.VoronoiLinkZones())
  
  -- создаем карту лестниц, указываем направление для каждой лестницы
  self.generator:AddLayer(luanet.VoronoiCreateStairs())
  
  -- генерируем тайлы и ди-тайлы, размещаем информацию о них в хранилище игровых объектов (storage)
  self.generator:AddLayer(luanet.VoronoiCubeLayer())
  
  -- выполняем настройку мира и назначаем заданные параметры категории мира для каждой позиции карты  
  self.generator:AddLayer(luanet.CreateWorldLayer(bioms, groupParams))   
  coroutine.yield()
  local summary_time = 0
  for i = 1, self.generator:GetCount() do
    local start_time = os.clock()
    self.generator:ApplyLayer(i - 1)
    summary_time = summary_time + (os.clock() - start_time)
    Console:Message("Layer " .. i .. " took a: " .. os.clock() - start_time .. " seconds")    
    coroutine.yield()
  end
  Console:Message("Summary time: " .. summary_time)  
  local scatterCoroutine = coroutine.create(function ()
    self:Scatter()
  end)
  local start_time = os.clock()
  while coroutine.status(scatterCoroutine) ~= 'dead' do
    coroutine.resume(scatterCoroutine, self)
    coroutine.yield()
  end  
  summary_time = os.clock() - start_time
  Console:Message("Scatter object during: " .. summary_time .. "s")  
  coroutine.yield()
  Console:Message("Init decals and billboards...")
  coroutine.yield()
  luanet.GameFacade.world:InitBillboards();
  luanet.GameFacade.world:InitDecals();  
  --luanet.GameFacade.world:WriteWorldDataToFile()
  Console:Message("World generated.")
end

function WorldGenerator:LoadWorld()
  luanet.World.CreateWorld(GameController.database.data.bioms, {GameController.database.data.groups.land, GameController.database.data.groups.land})
  --luanet.GameFacade.world:InitBillboards();
  --luanet.GameFacade.world:InitDecals();  
end

function WorldGenerator:Scatter()  
  local world = luanet.GameFacade.world
  local scattering = world.scattering
  local scatterTbl = GameController.database.data.scatter
  -- Get biom squares  
  local tmpClassTable = {}
  Console:Message("Scatter objects..")
  for k, v in pairs(scatterTbl) do
    Console:Message("Scatter " .. k .. " started..")
    coroutine.yield()
    tmpClassTable[k] = require(GameController.database.data.prefabs[k].require)
    for biomIdx, biom in pairs(v.biom) do
      local biomSquare = luanet.GameFacade.world:GetBiomSquare(biom)
      local scatterType = v.biom_scatter_type[biomIdx]
      local count
      if scatterType == 'stepping' then
        count = v.biom_count[biomIdx]
      else
        count = math.floor(v.biom_count[biomIdx] * biomSquare)
      end
      if count > 0 then
        local radius = GameController.database.data.prefabs[k].radius or 1        
        local scatterMethod = "ScatterRandomLua"
        if scatterType == "random" then
          scatterMethod = "ScatterRandomLua"
        elseif scatterType == "billow" then
          scatterMethod = "ScatterBillowLua"
        elseif scatterType == "stepping" then
          scatterMethod = "ScatterSteppingLua"
        end
        scattering[scatterMethod](scattering, biom, count, radius, k, v.fallScore or 5)
      end
    end
  end
  coroutine.yield()
  Console:Message("Finalize scattering..")
  coroutine.yield()
  local callCount = 0
  local callbackFunction = function (x, y, z, entityName)
    local storage = luanet.Whalebox.WorldStream.Grid.SpaceStorage()
    tmpClassTable[entityName].Create(storage, x, y, z, scatterTbl[entityName])
    storage:SetString("type", entityName)
    GameController.worldController:RegisterStorage(storage)        
    callCount = callCount + 1
  end
  scattering:Finalize(callbackFunction)
  coroutine.yield()
  Console:Message("Scatter " .. callCount .. " objects")
  coroutine.yield()
end
return WorldGenerator