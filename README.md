# CardAtlas

Index your Magic the Gathering collection with location tags.
Add custom proxies to your collection.
Uses scryfall's API for getting official WOTC card information, card prices etc.

Is meant for hosting on your own server. May eventually be hosted on official site.

## Restrictions / data decisions
- Cards do not contain rulings. <br/>
The scryfall api exposes rulings from the gatherer and their own (since gatherer does not have rulings for unofficial formats such as duel commander). For now I don't think this will be missed or needed, but that could change down the line. Anyway, for now it's not included.

- Updating card imagery from the scryfall api <br/>
The scryfall api provides imagery for all cards and are returned as an object. In this project card imagery is it's own table and can have multiple instances of everty type of card image.
Now, when updating the imagery from the api data it's been decided that we upsert each image type (Png, art crop, border crop etc.) by returning the first found instance of an image with that type which also correlates to the card and is provided by the api. This may or may not cause some issues down the line, if/when it becomes and issue, it will be handled then.

## Commands
Run the `docker-compose.yml` script detached (no logs).
- `docker-compose up -d`

List running docker containsers.
- `docker ps`

Stop the docker container.
- `docker-compose down`

Install Entity Framework.
- `dotnet tool install --global dotnet-ef`

Add new Migration:
- `dotnet ef Migrations add {migrationName}`

Update database with Migrations:
- `dotnet ef Database update`
