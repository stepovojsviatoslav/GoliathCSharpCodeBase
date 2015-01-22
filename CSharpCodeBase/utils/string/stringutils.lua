
StringUtils = {}

function StringUtils.split(str, sep)
    if sep == nil then
        sep = ','
    end
    
    if str == nil then
        return {}
    end
 
    local parts = {} --parts array
    local first = 1
    local ostart, oend = string.find(str, sep, first, true) --regexp disabled search
 
    while ostart do
        local part = string.sub(str, first, ostart - 1)
        table.insert(parts, part)
        first = oend + 1
        ostart, oend = string.find(str, sep, first, true)
    end
 
    local part = string.sub(str, first)
    table.insert(parts, part)
 
    return parts
end
