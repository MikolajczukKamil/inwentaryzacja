const {
  watch,
  statSync,
  readdirSync,
  readFileSync,
  writeFileSync,
  appendFileSync,
} = require('fs')
const { resolve: resolvePath } = require('path')

function From(relativePath) {
  return resolvePath(__dirname, relativePath)
}

function Combinates(param) {
  return [
    param.charAt(0),
    `-${param.charAt(0)}`,
    `--${param.charAt(0)}`,
    param,
    `-${param}`,
    `--${param}`,
  ]
}

const dataFile = From('./dist/db-data.sql')
const proceduresFile = From('./dist/db-procedures.sql')
const functionsFile = From('./dist/db-functions.sql')
const allFile = From('./dist/db-full.sql')

const minModeValues = Combinates('min')
const watchModeValues = Combinates('watch')

const minMode = process.argv.some((r) => minModeValues.includes(r))
const watchMode = process.argv.some((r) => watchModeValues.includes(r))

function FilesIn(dir, filelist = []) {
  readdirSync(dir).forEach((el) => {
    const file = resolvePath(dir, el)

    if (statSync(file).isDirectory()) {
      filelist = FilesIn(file, filelist)
    } else {
      filelist.push(file)
    }
  })

  return filelist
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

function BuildData(message = true, time = true) {
  let timeStart = Date.now()

  writeFileSync(dataFile, '')

  appendFileSync(dataFile, PrepereData(readFileSync(From('./data/tables.sql'))))
  appendFileSync(
    dataFile,
    PrepereData(readFileSync(From('./data/fake-content.sql')))
  )

  if (message) {
    console.log(
      `Data files${
        time ? ` completed in ${(Date.now() - timeStart) / 1000}s` : ''
      }`
    )
  }
}

function BuildProcedures(message = true, time = true) {
  let timeStart = Date.now()

  writeFileSync(proceduresFile, '')

  FilesIn(From('./Procedures')).forEach((file) => {
    appendFileSync(proceduresFile, PrepereData(readFileSync(file)))
  })

  if (message) {
    console.log(
      `Procedures files${
        time ? ` completed in ${(Date.now() - timeStart) / 1000}s` : ''
      }`
    )
  }
}

function BuildFunctions(message = true, time = true) {
  let timeStart = Date.now()

  writeFileSync(functionsFile, '')

  FilesIn(From('./Functions')).forEach((file) => {
    appendFileSync(functionsFile, PrepereData(readFileSync(file)))
  })

  if (message) {
    console.log(
      `Functions files${
        time ? ` completed in ${(Date.now() - timeStart) / 1000}s` : ''
      }`
    )
  }
}

function BuildFull(message = true, time = true) {
  writeFileSync(allFile, '')

  appendFileSync(allFile, readFileSync(dataFile))
  appendFileSync(allFile, readFileSync(functionsFile))
  appendFileSync(allFile, readFileSync(proceduresFile))
}

console.log('')

if (minMode) {
  console.info('Min mode ON')
}

if (!watchMode) {
  const timeStart = Date.now()

  BuildData(true, true)
  BuildProcedures(true, true)
  BuildFunctions(true, true)

  BuildFull(true, true)

  console.log(`Finished in ${(Date.now() - timeStart) / 1000}s`)
  console.log('')
}

if (watchMode) {
  console.info('Watch mode ON \n')

  const data = Symbol()
  const procedurse = Symbol()
  const functions = Symbol()
  let rebuildParts = [data, procedurse, functions]

  watch(From('./data'), (eventType, filename) => {
    if (!rebuildParts.includes(data)) rebuildParts.push(data)
  })

  watch(From('./Procedures'), (eventType, filename) => {
    if (!rebuildParts.includes(procedurse)) rebuildParts.push(procedurse)
  })

  watch(From('./Functions'), (eventType, filename) => {
    if (!rebuildParts.includes(functions)) rebuildParts.push(functions)
  })

  setInterval(() => {
    if (rebuildParts.length !== 0) {
      console.log(new Date().toLocaleString())

      rebuildParts.forEach((type) => {
        if (type === data) BuildData(true, false)
        if (type === procedurse) BuildProcedures(true, false)
        if (type === functions) BuildFunctions(true, false)
      })

      BuildFull(true, false)

      console.log('')

      rebuildParts = []
    }
  }, 1000)
}
