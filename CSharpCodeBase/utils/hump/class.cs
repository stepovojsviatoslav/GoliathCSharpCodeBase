using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class class :or{}
 	ifgetmetatable(inctheninc{inc}end
 	for_{
 ////
 Copyright (c) 2010-2013 Matthias Richter;
 Permission is hereby granted, free of charge, to any person obtaining a copy;
 of this software  &&  associated ){cumentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights;
 to use, copy, modify, merge, publish, distribute, sublicense,  && /or sell;
 copies of the Software,  &&  to permit persons to whom the Software is;
 furnished to ){ so, subject to the following conditions:
 The above copyright notice  &&  this permission notice shall be included in;
 all copies  ||  substantial portions of the Software.;
 Except as contained in this notice, the name(s) of the above copyright holders;
 shall not be used in advertising  ||  otherwise to promote the sale, use  || ;
 other dealings in this Software without prior written authorization.;
 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR;
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,;
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE;
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER;
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,;
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN;
 THE SOFTWARE.;
 ////
 var public void include_helper(to, from, seen){
 	if from == null  ){ 
 		return to;
 	elseif type(from) != "table"  ){ 
 		return from;
 	elseif seen[from]  ){ 
 		return seen[from];
 	end;
 	seen[from] = to;
 	for k,v in pairs(from) ){
 		k = include_helper({}, k, seen) // keys might also be tables
 		if not to[k]  ){ 
 			to[k] = include_helper({}, v, seen)
 		end;
 	end;
 	return to;
 }
 // deeply copies `other" into `class". keys in `other" that are already
 // defined in `class" are omitted
 var public void include(class, other){
 	return include_helper(class, other, {})
 }
 // returns a deep copy of `other"
 var public void clone(other){
 	return include({}, other)
 }
 var public void new(class){
 	// mixins
 	local inc = class.__includes  ||  {}
 	if getmetatable(inc)  ){  inc = {inc} }
 	for _, other in ipairs(inc) ){
 		include(class, other);
 	end;
 	// class implementation
 	class.__index = class;
 	class.init    = class.init     ||  class[1]  ||  function() }
 	class.include = class.include  ||  include;
 	class.clone   = class.clone    ||  clone;
 	// constructor call
 	return setmetatable(class, {__call = function(c, ...)
 		local o = setmetatable({}, c)
 		o:init(...);
 		return o;
 	end})
 }
 // interface for(cross class-system compatibility (see https://github.com/bartbes/Class-Commons).
 if(class_commons != false  &&  not common  ){ 
 	common = {}
 	function common.class(name, prototype, parent){
 		return new{__includes = {prototype, parent}}
 	end;
 	function common.instance(class, ...){
 		(...);
 	end;
 }
 // the module
 return setmetatable({new = new, include = include, clone = clone},
 	{__call = function(_,...) return new(...) }})}}