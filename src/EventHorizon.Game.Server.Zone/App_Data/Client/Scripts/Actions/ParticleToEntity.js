/**
 * Action Id: particle_to_entity
 *
 * $services:
 * - EntityRepository
 * $utils:
 * - sendEvent(eventName: string, data?: any)
 * $data:
 * - startEntityId: number
 * - endingEntityId: number
 * - particleTemplateId: string
 * - duration: number
 */

// TODO: Flush this out more.
const startingEntity = $services.entityRepository.get($data.startEntityId);
const endingEntity = $services.entityRepository.get($data.endingEntityId);
// Create entity at Starting Location
const particleEntity = $services.entityBuilder.createParticleEmitter(
    "Particle_Flame", // TODO: Move this Skill Configuration
    startingEntity.position,
    // Speed
    3 // TODO: Move this Skill Configuration
);
// Set to where the particle should move
particleEntity.moveTo(endingEntity.position);

// Remove particleEntity after duration
setTimeout(() => particleEntity.stop(), 3000); // TODO: Move this to Skill configuration
setTimeout(() => particleEntity.dispose(), 5000); // TODO: Move this to Skill configuration
