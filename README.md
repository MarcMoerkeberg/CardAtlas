# CardAtlas

Index your Magic the Gathering collection with location tags.
Add custom proxies to your collection.
Uses scryfall's API for getting official WOTC card information, card prices etc.

Is meant for hosting on your own server. May eventually be hosted on official site.

## Restrictions / data decisions
- Cannot create custom formats. <br/>
Due to how I would like to couple each card with a legality for each format. Then every time you add a new format, every card would have to be updated for the legality of that format. I think this will be a niche usecase and will not warrent payoff for the expected time implementing this (+ parsing data wtc.).

- Cards do not contain rulings. <br/>
The scryfall api exposes rulings from the gatherer and their own (since gatherer does not have rulings for unofficial formats such as duel commander). For now I don't think this will be missed or needed, but that could change down the line. Anyway, for now it's not included.
