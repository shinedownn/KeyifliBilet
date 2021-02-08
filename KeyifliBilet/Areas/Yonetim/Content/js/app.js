var client = algoliasearch("4J2JTO3FLK", "2f4cff0198aca3ee49fffa6df138e260")
var players = client.initIndex('players');
var teams = client.initIndex('teams');

autocomplete('#aa-search-input', {}, [
    {
        source: autocomplete.sources.hits(players, { hitsPerPage: 3 }),
        displayKey: 'name',
        templates: {
            header: '<div class="aa-suggestions-category">Players</div>',
            suggestion: function (suggestion) {
                return '<span>' +
                    suggestion._highlightResult.name.value + '</span><span>'
                    + suggestion._highlightResult.team.value + '</span>';
            }
        }
    },
    {
        source: autocomplete.sources.hits(teams, { hitsPerPage: 3 }),
        displayKey: 'name',
        templates: {
            header: '<div class="aa-suggestions-category">Teams</div>',
            suggestion: function (suggestion) {
                return '<span>' +
                    suggestion._highlightResult.name.value + '</span><span>'
                    + suggestion._highlightResult.location.value + '</span>';
            }
        }
    }
]);