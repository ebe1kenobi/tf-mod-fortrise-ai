from towerfall import Towerfall

def main():
  # Creates or reuse a Towerfall game.
  towerfall = Towerfall(
    verbose = 0,
    config = dict(
      matchLengths='Quick',
      mode='HeadHunters', #Quest, DarkWorld, Trials, LastManStanding, HeadHunters, TeamDeathmatch, ( not supported = Warlord)
      level=1,
      # fps=60,
      agentTimeout='24:00:00',      
      agents=[
        dict(type='remote', archer='white', ai='SimpleAgentLevel1'), 
        dict(type='remote', archer='white', ai='SimpleAgentLevel1'), 
        dict(type='remote', archer='orange', ai='SimpleAgentLevel1'),
        dict(type='remote', archer='orange', ai='SimpleAgentLevel1'),
        ],
    )
  )

  towerfall.run()

if __name__ == '__main__':
  main()