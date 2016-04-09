using System;

/// <summary>
/// An artificial intelligence attached to a team, for shepherding units.
/// </summary>
public class TeamAI {
	
	/// <summary>The team attached to this AI.</summary>
	public Team team;

	public TeamAI(Team team) {
		this.team = team;
	}

	public void calc(){
		//TODO check all team spawn points and spawn if possible.

		//For each unit on team.
		foreach(UnitScript u in team.units){
			//Make sure it is an AI unit.
			if(u.ai == null)
				continue;
			//Set behavior probabilities.
			double moveDir = 0.5;
			double moveIdle = 0;
			double moveScope = 0.5;
			double moveGlobalScope = 0.5;
			double moveTarget = 0.5;
			double moveTeam = 0.5;
			double attack = 0.5;

			//TODO logic

			u.ai.moveDir=moveDir;
			u.ai.moveIdle=moveIdle;
			u.ai.moveScope=moveScope;
			u.ai.moveGlobalScope=moveGlobalScope;
			u.ai.moveTarget=moveTarget;
			u.ai.moveTeam=moveTeam;
			u.ai.attack=attack;
		}
	}
}