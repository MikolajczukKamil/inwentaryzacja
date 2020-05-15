const {
  writeFileSync,
  readdirSync,
  readFileSync,
  appendFileSync
} = require('fs')

const minMode = process.argv.includes('-min')

if (minMode) {
  console.info('Min mode ON')
}

function PrepereData(data) {
  if (minMode) {
    return data
      .toString()
      .replace(/\/\*[\s\S]*?\*\//g, '')
      .replace(/^\s{2,}/gm, '')
  }

  return data
}

const dataFile = './dist/db-data.sql'

writeFileSync(dataFile, '')

appendFileSync(dataFile, PrepereData(readFileSync('./data/tables.sql')))
appendFileSync(dataFile, PrepereData(readFileSync('./data/fake-content.sql')))

console.log('Data files completed')

const proceduresFile = './dist/db-procedures.sql'

writeFileSync(proceduresFile, '')

Array()
  .concat(
    readdirSync('./Functions').map((file) => `./Functions/${file}`),
    readdirSync('./Procedures').map((file) => `./Procedures/${file}`)
  )
  .forEach((file) => {
    appendFileSync(proceduresFile, PrepereData(readFileSync(file)))
  })

console.log('Functions and Procedures files completed')

const allFile = './dist/db-full.sql'

writeFileSync(allFile, '')

appendFileSync(allFile, readFileSync(dataFile))
appendFileSync(allFile, readFileSync(proceduresFile))

console.log('All files completed')
