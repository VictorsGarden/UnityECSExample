Архитектура проекта сделана через Unity ECS редакции за июнь 2020

ECS — Entity Component System ECS — это шаблон проектирования "Сущность/ Компонент/ Система"<br />
Сущности (Entity) — объекты-контейнеры, не обладающие свойствами, но выступающие хранилищами для Компонентов.<br />

Компоненты — это блоки данных, определяющие всевозможные свойства любых игровых объектов или событий. Все эти данные, сгруппированные в контейнеры, обрабатываются логикой, существующей исключительно в виде Систем<br />
Системы — чистые классЫ с определенными методами для выполнения.<br />

Unity ECS:<br />
Job System реализованы через Entities.ForEach<br />
Поведение реализованно через системы (Systems), работающие на основе класса SystemBase<br />

Для повышения производительности и возможности параллельной работы используется Burst компилятор.<br />
Для отключения Burst компилятора необходимо сделать постфикс .WithoutBurst() и запустить через .Run()<br />
Для включения Burst компилятора постфикс не нужен, но можно написать .WithBurst() и запустить через .Schedule()<br />
при включенном Burst компиляторе нельзя обращаться к глобальной зоне видимости (Time.DeltaTime) или изменять значения из внешней зоны видимости<br />

Entities (Сущности):<br />
инициализируются через AddComponentData() класса EntityManager<br />
- CubeEntity<br />
- FinishZoneEntity<br />
- MagnetEntity<br />

Components (Компоненты):<br />
Являются вместителями состояния Сущностей<br />

- CubeData:<br />
	CubeType – Тип,<br />
	Direction – Направление,<br />
	StickingPoint – Точка отбивания от Углов,<br />
	Acceleration – Ускорение,<br />
	IsAccelerated – Ускорен,<br />
	IsReflected – Отбит,<br />
	IsCollected – Собран<br />
	
- FinishZoneData<br />
	 ZoneType – Тип (какие кубики собирает),<br />
	 MaxCount – Максимальное Количество (для сбора),<br />
	 Horizontal, Vertical – Горизонтальные и Вертикальные оси координат (для просчета вхождений)<br />
	 HasCollected – Собрана ли зона<br />
	 
 - MagnetTag<br />
	содержит пользовательский ввод и время последнего кадра

Системы:
	
- PlayerInputSystem<br />
	Читает пользовательский ввод и записывает его в данные компонента магнита
	
- MagnetMovingSystem<br />
	Двигает магнит на основе данных пользовательского ввода

- AccelerateSystem<br />
	отвечает за отталкивание кубиков от магнита по заданному расстоянию<br />
	задает ускорение и направление (относительно магнита), если кубик находится в магнитном поле<br />

- CubesMovingSystem<br />
	двигает кубики, если у них есть ускорение,<br />
	а также плавно уменьшает ускорение со временем (чтобы останавливать разогнанные кубики)

- *CubesReflectionSystem<br />
	отбивает кубики от стен<br />
	защищает кубики от вылетов за стены – ProtectFromOutbounding()<br />
	отбивает кубики от углов, заданных определенными координатами, чтобы кубики не скапливались в углах<br />

	например, чтобы отбить кубик от левой стены:<br />
	мы проверяем, заходит ли его положение за левую координату огранчений,<br />
	если да, то отзеркаливаем его Direction от правой нормали<br />

- FinishZoneSystem<br />
	Проверяет, входят ли кубики в заданные координаты и останавливает их,<br />
	постепенно притягивая их к центру (чтобы они не останавливались по краям финиш зоны)<br />
	Считает собранные кубики и наращивает счетчик в данные Finish зоны<br />
	Если кубики собрались ставит флаг IsCollected как true<br />
	
- CheckFinishZoneSystem<br />
	проверяет финиш зону на IsCollected, если он true, делает нужное событие<br />

________________________________

* Процедурная замена работы коллизий от стен