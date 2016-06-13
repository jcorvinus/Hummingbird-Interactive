# Hummingbird Interactive
This is a short prototype experience for Microsoft's HoloLens, made during a #HoloHacks hackathon. The ultimate goal to provide a way for visitors to an educational space to learn about various plants and animals in an engaging, interactive way.

For now, the program provides you with a single bird that shows an informational dialog when selected. You can then play around with the bird by issuing it commands.

Voice commands:
"Select this bird" - if a bird is under the cursor, it will be selected.
"Land over there" - If a bird is selected, and the cursor is green, the bird will land on the selected surface.
"Fly over there" - Sends the selected bird to open space a certain distance from the headset. No cursor required.
"Come to me" - The selected bird will fly up to you and stop a little over arm's length away, hovering in place.
"Sing to me" - The selected bird will sing a short song.
"Make Bird - The program will spawn a new bird.
"Make Ten Birds" - The program will spawn ten new birds.
"Kill All Birds" - All birds will be despawned
"Chase Me" - All birds will chase you.
"Stop Chasing Me" - All birds will stop chasing you and circle in place.

Known issues:
Sometimes the program will start but not render anything. You can usually catch this bug right away by looking to see if the white "made with unity" text appears. If you hear a hummingbird but don't see anything, close the program and restart.