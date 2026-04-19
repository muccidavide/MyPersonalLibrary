<script setup>
import { reactiveOmit } from "@vueuse/core";
import { PaginationRoot, useForwardPropsEmits } from "reka-ui";
import { cn } from "@/lib/utils";

const props = defineProps({
  page: { type: Number, required: false },
  defaultPage: { type: Number, required: false },
  itemsPerPage: { type: Number, required: true },
  total: { type: Number, required: false },
  siblingCount: { type: Number, required: false },
  disabled: { type: Boolean, required: false },
  showEdges: { type: Boolean, required: false },
  asChild: { type: Boolean, required: false },
  as: { type: null, required: false },
  class: { type: null, required: false },
});
const emits = defineEmits(["update:page"]);

const delegatedProps = reactiveOmit(props, "class");
const forwarded = useForwardPropsEmits(delegatedProps, emits);
</script>

<template>
  <PaginationRoot
    v-slot="slotProps"
    data-slot="pagination"
    v-bind="forwarded"
    :class="cn('mx-auto flex w-full justify-center fixed bottom-0 left-0 right-0 z-40 bg-white/90 dark:bg-gray-900/90 backdrop-blur-md py-3 border-t border-gray-200 dark:border-gray-800 sm:static sm:bg-transparent sm:dark:bg-transparent sm:backdrop-blur-none sm:py-0 sm:border-none', props.class)"
  >
    <slot v-bind="slotProps" />
  </PaginationRoot>
</template>
