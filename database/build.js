const { writeFileSync, readdirSync, readFileSync, appendFileSync } = require('fs')
const path = require('path')

function PrepereData(data) {
  if (minMode) {
    return data
      .toString()
      .replace(/\/\*[\s\S]*?\*\//g, '')
      .replace(/^\s{2,}/gm, '')
  }

  return data
}

function From(p) {
  return path.resolve(__dirname, p)
}

console.log('')

const minMode = process.argv.includes('-min') || process.argv.includes('min') || process.argv.includes('--min')

if (minMode) {
  console.info('Min mode ON')
}

const startGlobal = Date.now()
let timeStart = startGlobal
let timeEnd = startGlobal

const dataFile = From('./dist/db-data.sql')

writeFileSync(dataFile, '')

appendFileSync(dataFile, PrepereData(readFileSync(From('./data/tables.sql'))))
appendFileSync(dataFile, PrepereData(readFileSync(From('./data/fake-content.sql'))))

timeEnd = Date.now()

console.log(`Data files completed in ${(timeEnd - timeStart) / 1000}s`)

timeStart = Date.now()

const proceduresFile = From('./dist/db-procedures.sql')

writeFileSync(proceduresFile, '')

Array()
  .concat(
    readdirSync(From('./Functions')).map((file) => From(`./Functions/${file}`)),
    readdirSync(From('./Procedures')).map((file) => From(`./Procedures/${file}`))
  )
  .forEach((file) => {
    appendFileSync(proceduresFile, PrepereData(readFileSync(file)))
  })

timeEnd = Date.now()

console.log(`Functions and Procedures files completed in ${(timeEnd - timeStart) / 1000}s`)

timeStart = Date.now()

const allFile = From('./dist/db-full.sql')

writeFileSync(allFile, '')

appendFileSync(allFile, readFileSync(dataFile))
appendFileSync(allFile, readFileSync(proceduresFile))

timeEnd = Date.now()

console.log(`Build db-full in ${(timeEnd - timeStart) / 1000}s\n`)

console.log(`Finished in ${(timeEnd - startGlobal) / 1000}s`)
