/**
 * Description: This script will create a Tree Mesh.
 *
 * "Services" Provided by Script system to help with external to script access.
 * $services: {
 *   i18n: I18nService;
 *   logger: ILogger;
 *   eventService: IEventService;
 *   commandService: ICommandService;
 *   babylonjs: BABYLONJS
 * };
 *
 * $utils: {
 *  isObjectDefined(obj: any): bool;
 *  createEvent(event: string, data?: any): IEvent;
 * }
 *
 * This the internal "state" of the script, only accessible by the script.
 * $state: {
 * };
 *
 * This is data passed to the script from the outside.
 * $data: {
 *  id: string;
 *  scene: Scene;
 *  branchSize: number;
 *  trunkSize: number;
 *  radius: number;
 * };
 */

return new Promise(resolve => {
    const BABYLON = $services.renderingApi;
    const { id, scene } = $data;
    // TODO: pull from $state or Asset store
    var leafMaterial = new BABYLON.StandardMaterial("leafMaterial", scene);
    leafMaterial.diffuseColor = new BABYLON.Color3(0.5, 1, 0.5);

    // TODO: pull from $state or Asset store
    var woodMaterial = new BABYLON.StandardMaterial("woodMaterial", scene);
    var woodTexture = new BABYLON.WoodProceduralTexture(
        "woodMaterial_text",
        512,
        scene
    );
    woodTexture.ampScale = 50;
    woodMaterial.diffuseTexture = woodTexture;

    const createQuickTreeGenerator = (sizeBranch, sizeTrunk, radius) => {
        var tree = new BABYLON.Mesh(`tree-${id}`, scene);
        tree.setEnabled(true);
        tree.isVisible = true;

        var leaves = new BABYLON.Mesh("leaves", scene);

        var vertexData = BABYLON.VertexData.CreateSphere({
            segments: 2,
            diameter: sizeBranch
        });

        vertexData.applyToMesh(leaves, false);

        var positions = leaves.getVerticesData(
            BABYLON.VertexBuffer.PositionKind
        );
        var indices = leaves.getIndices();
        var numberOfPoints = positions.length / 3;

        var map = [];

        // The higher point in the sphere
        var v3 = BABYLON.Vector3;
        var max = [];

        for (var i = 0; i < numberOfPoints; i++) {
            var p = new v3(
                positions[i * 3],
                positions[i * 3 + 1],
                positions[i * 3 + 2]
            );

            if (p.y >= sizeBranch / 2) {
                max.push(p);
            }

            var found = false;
            for (var index = 0; index < map.length && !found; index++) {
                var array = map[index];
                var p0 = array[0];
                if (p0.equals(p) || p0.subtract(p).lengthSquared() < 0.01) {
                    array.push(i * 3);
                    found = true;
                }
            }
            if (!found) {
                var array = [];
                array.push(p, i * 3);
                map.push(array);
            }
        }
        var randomNumber = function(min, max) {
            if (min == max) {
                return min;
            }
            var random = Math.random();
            return random * (max - min) + min;
        };

        map.forEach(function(array) {
            var index,
                min = -sizeBranch / 10,
                max = sizeBranch / 10;
            var rx = randomNumber(min, max);
            var ry = randomNumber(min, max);
            var rz = randomNumber(min, max);

            for (index = 1; index < array.length; index++) {
                var i = array[index];
                positions[i] += rx;
                positions[i + 1] += ry;
                positions[i + 2] += rz;
            }
        });

        leaves.setVerticesData(BABYLON.VertexBuffer.PositionKind, positions);
        var normals = [];
        BABYLON.VertexData.ComputeNormals(positions, indices, normals);
        leaves.setVerticesData(BABYLON.VertexBuffer.NormalKind, normals);
        leaves.convertToFlatShadedMesh();

        leaves.material = leafMaterial;
        leaves.position.y = sizeTrunk + sizeBranch / 2 - 2;

        var trunk = BABYLON.Mesh.CreateCylinder(
            "trunk",
            sizeTrunk,
            radius - 2 < 1 ? 1 : radius - 2,
            radius,
            10,
            2,
            scene
        );

        trunk.position.y = sizeBranch / 2 + 2 - sizeTrunk / 2;

        trunk.material = woodMaterial;
        trunk.convertToFlatShadedMesh();

        leaves.parent = tree;
        trunk.parent = tree;
        return tree;
    };
    resolve(
        createQuickTreeGenerator(
            $data.branchSize,
            $data.trunkSize,
            $data.radius
        )
    );
});
