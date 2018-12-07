// Services:
// - EntityRepository
// Methods:
// - sendEvent(eventName: string, data?: any)
// Data: 
// - startEntityId: number
// - endingEntityId: number
// - particleTemplateId: string 
// - duration: number

// TODO: Flush this out more.
const startingEntity = Services.EntityRepository.get(
	Data.startEntityId
);
const endingEntity = Services.EntityRepository.get(
	Data.endingEntityId
);
// Create entity at Starting Location
const particleEntity = Services.EntityBuilder.createParticleEmitter(
	"Flame_Particle", // TODO: Move this Skill Configuration
	startingEntity.position,
	2 // TODO: Move this Skill Configuration
);
// Set to where the particle should move
particleEntity.moveTo(endingEntity.position)

// Remove particleEntity after duration
setTimeout(() => particleEntity.stop(), 3000); // TODO: Move this to Skill configuration
setTimeout(() => particleEntity.dispose(), 5000); // TODO: Move this to Skill configuration